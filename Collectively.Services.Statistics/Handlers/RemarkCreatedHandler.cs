using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Repositories;

namespace Collectively.Services.Statistics.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private readonly IHandler _handler;
        private readonly IRemarkStatisticsRepository _remarkStatisticsRepository;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly ICategoryStatisticsRepository _categoryStatisticsRepository;
        private readonly ITagStatisticsRepository _tagStatisticsRepository;

        public RemarkCreatedHandler(IHandler handler, 
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

        public async Task HandleAsync(RemarkCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    await HandleRemarkStatisticsAsync(@event);
                    await HandleUserStatisticsAsync(@event);
                    await HandleCategoryStatisticsAsync(@event);
                    await HandleTagStatisticsAsync(@event);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {typeof(RemarkCreated).Name} event"))
                .ExecuteAsync();
        }

        private async Task HandleRemarkStatisticsAsync(RemarkCreated @event)
        {
            var remarkStatistics = await _remarkStatisticsRepository.GetAsync(@event.RemarkId);
            if (remarkStatistics.HasNoValue)
            {
                remarkStatistics = new RemarkStatistics(@event.RemarkId,
                    @event.Category.Name, @event.UserId, @event.Username,
                    @event.CreatedAt, @event.State.State, 
                    @event.Location.Latitude, @event.Location.Longitude, 
                    @event.Location.Address, @event.Description, @event.Tags);
            }

            await _remarkStatisticsRepository.AddOrUpdateAsync(remarkStatistics.Value);
        }

        private async Task HandleUserStatisticsAsync(RemarkCreated @event)
        {
            var userStatistics = await _userStatisticsRepository.GetByIdAsync(@event.UserId);
            if (userStatistics.HasNoValue)
            {
                userStatistics = new UserStatistics(@event.UserId, @event.Username,
                    new RemarksCountStatistics(@new: 1, reported: 1));
            }

            userStatistics.Value.Remarks.IncreaseReported();
            await _userStatisticsRepository.AddOrUpdateAsync(userStatistics.Value);
        }

        private async Task HandleCategoryStatisticsAsync(RemarkCreated @event)
        {
            var categoryStatistics = await _categoryStatisticsRepository.GetByNameAsync(@event.Category.Name);
            if (categoryStatistics.HasNoValue)
            {
                categoryStatistics = new CategoryStatistics(@event.Category.Name, 
                    new RemarksCountStatistics(@new: 1, reported: 1));
            }

            categoryStatistics.Value.Remarks.IncreaseReported();
            await _categoryStatisticsRepository.AddOrUpdateAsync(categoryStatistics.Value);
        }

        private async Task HandleTagStatisticsAsync(RemarkCreated @event)
        {
            if (@event.Tags == null || @event.Tags.Any() == false)
                return;

            foreach (var tag in @event.Tags)
            {
                var tagStatistic = await _tagStatisticsRepository.GetByNameAsync(tag);
                if (tagStatistic.HasNoValue)
                {
                    tagStatistic = new TagStatistics(tag, new RemarksCountStatistics(@new: 1, reported: 1));
                }

                tagStatistic.Value.Remarks.IncreaseReported();
                await _tagStatisticsRepository.AddOrUpdateAsync(tagStatistic.Value);
            }
        }
    }
}