using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories;

namespace Coolector.Services.Statistics.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IHandler _handler;
        private readonly IUserStatisticsRepository _repository;

        public RemarkResolvedHandler(IHandler handler, IUserStatisticsRepository repository)
        {
            _handler = handler;
            _repository = repository;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            await _handler
                .Run(async () =>
                {
                    var userStatistics = await _repository.GetByIdAsync(@event.UserId);
                    if (userStatistics.HasNoValue)
                        userStatistics = new UserStatistics(@event.UserId, @event.Username);

                    userStatistics.Value.IncreaseResolvedCount();

                    await _repository.UpsertAsync(userStatistics.Value);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {typeof(RemarkResolved).Name} event"))
                .ExecuteAsync();
        }
    }
}