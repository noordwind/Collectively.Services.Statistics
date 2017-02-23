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
        private readonly ICategoryStatisticsRepository _categoryStatisticsRepository;
        private readonly ITagStatisticsRepository _tagStatisticsRepository;

        public RemarkResolvedHandler(IHandler handler, 
            IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository,
            ICategoryStatisticsRepository categoryStatisticsRepository,
            ITagStatisticsRepository tagStatisticsRepository)
        {
            _handler = handler;
            _remarkStatisticsRepository = remarkStatisticsRepository;
            _userStatisticsRepository = userStatisticsRepository;
            _categoryStatisticsRepository = categoryStatisticsRepository;
            _tagStatisticsRepository = tagStatisticsRepository;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            await _handler
                .Run(async () =>
                {
                    await HandleRemarkStatisticsAsync(@event);
                    await HandleUserStatisticsAsync(@event);
                    await HandleCategoryStatisticsAsync(@event);
                    await HandleTagStatisticsAsync(@event);
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
            if (@event.State.Location != null)
            {
                location = new RemarkStatistics.RemarkLocation(@event.State.Location.Latitude,
                    @event.State.Location.Longitude, @event.State.Location.Address);
            }

            remarkStatistics.Value.SetResolved(@event.UserId, @event.State.Username, @event.State.CreatedAt, location);
            await _remarkStatisticsRepository.AddOrUpdateAsync(remarkStatistics.Value);
        }

        private async Task HandleUserStatisticsAsync(RemarkResolved @event)
        {
            var userStatistics = await _userStatisticsRepository.GetByIdAsync(@event.UserId);
            if (userStatistics.HasNoValue)
            {
                userStatistics = new UserStatistics(@event.UserId, @event.State.Username);
            }

            userStatistics.Value.IncreaseResolved();
            await _userStatisticsRepository.AddOrUpdateAsync(userStatistics.Value);
        }

        private async Task HandleCategoryStatisticsAsync(RemarkResolved @event)
        {
            var remarkStats = await _remarkStatisticsRepository.GetAsync(@event.RemarkId);
            if (remarkStats.HasNoValue)
                return;

            var categoryStats = await _categoryStatisticsRepository.GetByNameAsync(remarkStats.Value.Category);
            if (categoryStats.HasNoValue)
            {
                categoryStats = new CategoryStatistics(remarkStats.Value.Category, 1U);
            }

            categoryStats.Value.IncreaseResolved();
            await _categoryStatisticsRepository.AddOrUpdateAsync(categoryStats.Value);
        }

        private async Task HandleTagStatisticsAsync(RemarkResolved @event)
        {
            var remarkStats = await _remarkStatisticsRepository.GetAsync(@event.RemarkId);
            if (remarkStats.HasNoValue)
                return;

            foreach (var tag in remarkStats.Value.Tags)
            {
                var tagStats = await _tagStatisticsRepository.GetByNameAsync(tag);
                if (tagStats.HasNoValue)
                {
                    tagStats = new TagStatistics(tag, 1U);
                }
                tagStats.Value.IncreaseResolved();
                await _tagStatisticsRepository.AddOrUpdateAsync(tagStats.Value);
            }
        }
    }
}