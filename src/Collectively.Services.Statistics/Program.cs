using Collectively.Common.Host;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Framework;

namespace Collectively.Services.Statistics
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
                .SubscribeToEvent<RemarkProcessed>()
                .SubscribeToEvent<RemarkRenewed>()
                .SubscribeToEvent<RemarkCanceled>()
                .SubscribeToEvent<RemarkVoteSubmitted>()
                .SubscribeToEvent<RemarkVoteDeleted>()
                .Build()
                .Run();
        }
    }
}
