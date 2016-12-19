using Coolector.Common.Host;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Statistics.Framework;

namespace Coolector.Services.Statistics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 10006)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToEvent<RemarkCreated>()
                .SubscribeToEvent<RemarkResolved>()
                .SubscribeToEvent<RemarkDeleted>()
                .Build()
                .Run();
        }
    }
}
