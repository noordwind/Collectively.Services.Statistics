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
        private readonly IRemarkStatisticsRepository _remarkStatisticsRepository;
        private readonly IUserStatisticsRepository _userStatisticsRepository;

        public RemarkResolvedHandler(IHandler handler, 
            IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository)
        {
            _handler = handler;
            _remarkStatisticsRepository = remarkStatisticsRepository;
            _userStatisticsRepository = userStatisticsRepository;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            await _handler
                .Run(async () =>
                {
                    await HandleRemarkStatisticsAsync(@event);
                    await HandleUserStatisticsAsync(@event);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {typeof(RemarkResolved).Name} event"))
                .ExecuteAsync();
        }

        private async Task HandleRemarkStatisticsAsync(RemarkResolved @event)
        {
            var remarkStatistics = await _remarkStatisticsRepository.GetAsync(@event.RemarkId);
            if (remarkStatistics.HasNoValue)
                return;

            RemarkStatistics.RemarkLocation location = null;
            if (@event.ResolvedAtLocation != null)
            {
                location = new RemarkStatistics.RemarkLocation(@event.ResolvedAtLocation.Latitude,
                    @event.ResolvedAtLocation.Longitude, @event.ResolvedAtLocation.Address);
            }

            remarkStatistics.Value.SetResolved(@event.UserId, @event.Username, @event.ResolvedAt, location);
            await _remarkStatisticsRepository.AddOrUpdateAsync(remarkStatistics.Value);
        }

        private async Task HandleUserStatisticsAsync(RemarkResolved @event)
        {
            var userStatistics = await _userStatisticsRepository.GetByIdAsync(@event.UserId);
            if (userStatistics.HasNoValue)
            {
                userStatistics = new UserStatistics(@event.UserId, @event.Username);
            }

            userStatistics.Value.IncreaseResolvedCount();
            await _userStatisticsRepository.AddOrUpdateAsync(userStatistics.Value);
        }
    }
}