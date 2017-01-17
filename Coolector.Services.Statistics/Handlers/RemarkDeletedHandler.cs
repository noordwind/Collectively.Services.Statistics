using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories;

namespace Coolector.Services.Statistics.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkStatisticsRepository _remarkStatisticsRepository;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly ICategoryStatisticsRepository _categoryStatisticsRepository;
        private readonly ITagStatisticsRepository _tagStatisticsRepository;

        public RemarkDeletedHandler(IHandler handler, 
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
        
        public async Task HandleAsync(RemarkDeleted @event)
        {
            await _handler
                .Run(async () =>
                {
                    await HandleRemarkStatisticsAsync(@event);
                    await HandleUserStatisticsAsync(@event);
                    await HandleCategoryStatisticsAsync(@event);
                    await HandleTagStatisticsAsync(@event);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {typeof(RemarkDeleted).Name} event"))
                .ExecuteAsync();
        }

        private async Task HandleRemarkStatisticsAsync(RemarkDeleted @event)
        {
            var remarkStatistics = await _remarkStatisticsRepository.GetAsync(@event.Id);
            if (remarkStatistics.HasNoValue)
                return;

            remarkStatistics.Value.SetDeleted();
            await _remarkStatisticsRepository.AddOrUpdateAsync(remarkStatistics.Value);
        }

        private async Task HandleUserStatisticsAsync(RemarkDeleted @event)
        {
            var userStatistics = await _userStatisticsRepository.GetByIdAsync(@event.UserId);
            if (userStatistics.HasNoValue)
                return;

            userStatistics.Value.IncreaseDeleted();
            await _userStatisticsRepository.AddOrUpdateAsync(userStatistics.Value);
        }

        private async Task HandleCategoryStatisticsAsync(RemarkDeleted @event)
        {
            var remarkStats = await _remarkStatisticsRepository.GetAsync(@event.Id);
            if (remarkStats.HasNoValue)
                return;

            var categoryStats = await _categoryStatisticsRepository.GetByNameAsync(remarkStats.Value.Category);
            if (categoryStats.HasNoValue)
            {
                categoryStats = new CategoryStatistics(remarkStats.Value.Category, 1U);
            }

            categoryStats.Value.IncreaseDeleted();
            await _categoryStatisticsRepository.AddOrUpdateAsync(categoryStats.Value);
        }

        private async Task HandleTagStatisticsAsync(RemarkDeleted @event)
        {
            var remarkStats = await _remarkStatisticsRepository.GetAsync(@event.Id);
            if (remarkStats.HasNoValue)
                return;

            foreach (var tag in remarkStats.Value.Tags)
            {
                var tagStats = await _tagStatisticsRepository.GetByNameAsync(tag);
                if (tagStats.HasNoValue)
                {
                    tagStats = new TagStatistics(tag, 1U);
                }
                tagStats.Value.IncreaseDeleted();
                await _tagStatisticsRepository.AddOrUpdateAsync(tagStats.Value);
            }
        }
    }
}