using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories;

namespace Coolector.Services.Statistics.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private readonly IHandler _handler;
        private readonly IReporterRepository _repository;

        public RemarkCreatedHandler(IHandler handler, IReporterRepository repository)
        {
            _handler = handler;
            _repository = repository;
        }

        public async Task HandleAsync(RemarkCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var reporter = await _repository.GetByNameAsync(@event.Username);
                    if (reporter.HasNoValue)
                        reporter = new Reporter(@event.UserId, @event.Username);
                    else
                        reporter.Value.IncreaseReportedCount();

                    await _repository.UpsertAsync(reporter.Value);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {typeof(RemarkCreated).Name} event"))
                .ExecuteAsync();
        }
    }
}