using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Statistics.Repositories;

namespace Coolector.Services.Statistics.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IHandler _handler;
        private readonly IUserStatisticsRepository _repository;

        public RemarkDeletedHandler(IHandler handler, IUserStatisticsRepository repository)
        {
            _handler = handler;
            _repository = repository;
        }
        
        public async Task HandleAsync(RemarkDeleted @event)
        {
            await _handler
                .Run(async () =>
                {
                    var userStatistics = await _repository.GetByIdAsync(@event.UserId);
                    if (userStatistics.HasNoValue)
                        return;

                    userStatistics.Value.DecreaseReportedCount();
                    await _repository.UpsertAsync(userStatistics.Value);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {typeof(RemarkDeleted).Name} event"))
                .ExecuteAsync();
        }
    }
}