using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Autofac;
using Coolector.Common.Events;
using Coolector.Common.Exceptionless;
using Coolector.Common.Extensions;
using Coolector.Common.Mongo;
using Coolector.Common.Nancy;
using Coolector.Common.Services;
using Coolector.Services.Statistics.Repositories;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using NLog;
using Polly;
using RabbitMQ.Client.Exceptions;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;

namespace Coolector.Services.Statistics.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IExceptionHandler _exceptionHandler;
        private static readonly string DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static readonly string InvalidDecimalSeparator = DecimalSeparator == "." ? "," : ".";
        private readonly IConfiguration _configuration;
        public static ILifetimeScope LifetimeScope { get; private set; }

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

#if DEBUG
        public override void Configure(INancyEnvironment environment)
        {
            base.Configure(environment);
            environment.Tracing(enabled: false, displayErrorTraces: true);
        }
#endif

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            Logger.Info("Coolector.Services.Statistics Configuring application container");
            base.ConfigureApplicationContainer(container);

            var rmqRetryPolicy = Policy
                .Handle<ConnectFailureException>()
                .Or<BrokerUnreachableException>()
                .Or<IOException>()
                .WaitAndRetry(5, retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Logger.Error(exception,
                            $"Cannot connect to RabbitMQ. retryCount:{retryCount}, duration:{timeSpan}");
                    }
                );

            container.Update(builder =>
            {
                builder.RegisterInstance(_configuration.GetSettings<MongoDbSettings>()).SingleInstance();
                builder.RegisterModule<MongoDbModule>();
                builder.RegisterType<MongoDbInitializer>().As<IDatabaseInitializer>();
                builder.RegisterType<DatabaseSeeder>().As<IDatabaseSeeder>();
                builder.RegisterInstance(_configuration.GetSettings<ExceptionlessSettings>()).SingleInstance();
                builder.RegisterType<ExceptionlessExceptionHandler>().As<IExceptionHandler>().SingleInstance();
                builder.RegisterType<Handler>().As<IHandler>();
                builder.RegisterType<ReporterRepository>().As<IReporterRepository>();
                builder.RegisterType<ResolverRepository>().As<IResolverRepository>();
                var rawRabbitConfiguration = _configuration.GetSettings<RawRabbitConfiguration>();
                builder.RegisterInstance(rawRabbitConfiguration).SingleInstance();
                rmqRetryPolicy.Execute(() => builder
                        .RegisterInstance(BusClientFactory.CreateDefault(rawRabbitConfiguration))
                        .As<IBusClient>()
                );
                var coreAssembly = typeof(Startup).GetTypeInfo().Assembly;
                builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
            });
            LifetimeScope = container;
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                _exceptionHandler.Handle(ex, ctx.ToExceptionData(),
                    "Request details", "Coolector", "Service", "Storage");

                return ctx.Response;
            });
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync();
            var databaseSeeder = container.Resolve<IDatabaseSeeder>();
            databaseSeeder.SeedAsync();

            pipelines.BeforeRequest += (ctx) =>
            {
                FixNumberFormat(ctx);

                return null;
            };

            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,PUT,GET,OPTIONS,DELETE");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers",
                    "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            _exceptionHandler = container.Resolve<IExceptionHandler>();
            Logger.Info("Coolector.Services.Statistics API has started.");
        }

        private void FixNumberFormat(NancyContext ctx)
        {
            if (ctx.Request.Query == null)
                return;

            var fixedNumbers = new Dictionary<string, double>();
            foreach (var key in ctx.Request.Query)
            {
                var value = ctx.Request.Query[key].ToString();
                if (!value.Contains(InvalidDecimalSeparator))
                    continue;

                var number = 0;
                if (int.TryParse(value.Split(InvalidDecimalSeparator[0])[0], out number))
                    fixedNumbers[key] = double.Parse(value.Replace(InvalidDecimalSeparator, DecimalSeparator));
            }
            foreach (var fixedNumber in fixedNumbers)
            {
                ctx.Request.Query[fixedNumber.Key] = fixedNumber.Value;
            }
        }
    }
}