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
        private readonly IUserStatisticsRepository _repository;

        public RemarkCreatedHandler(IHandler handler, IUserStatisticsRepository repository)
        {
            _handler = handler;
            _repository = repository;
        }

        public async Task HandleAsync(RemarkCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var userStatistics = await _repository.GetByIdAsync(@event.UserId);
                    if (userStatistics.HasNoValue)
                        userStatistics = new UserStatistics(@event.UserId, @event.Username);

                    userStatistics.Value.IncreaseReportedCount();

                    await _repository.UpsertAsync(userStatistics.Value);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {typeof(RemarkCreated).Name} event"))
                .ExecuteAsync();
        }
    }
}