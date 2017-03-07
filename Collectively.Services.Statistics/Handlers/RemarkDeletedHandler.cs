using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Repositories;
using Collectively.Common.ServiceClients;

namespace Collectively.Services.Statistics.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkStatisticsRepository _remarkStatisticsRepository;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly ICategoryStatisticsRepository _categoryStatisticsRepository;
        private readonly ITagStatisticsRepository _tagStatisticsRepository;
        private readonly IServiceClient _serviceClient;

        public RemarkDeletedHandler(IHandler handler, 
            IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository,
            ICategoryStatisticsRepository categoryStatisticsRepository,
            ITagStatisticsRepository tagStatisticsRepository,
            IServiceClient serviceClient)
        {
            _handler = handler;
            _remarkStatisticsRepository = remarkStatisticsRepository;
            _userStatisticsRepository = userStatisticsRepository;
            _categoryStatisticsRepository = categoryStatisticsRepository;
            _tagStatisticsRepository = tagStatisticsRepository;
            _serviceClient = serviceClient;
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

            remarkStatistics.Value.AddState(new RemarkState("deleted", @event.UserId));
            await _remarkStatisticsRepository.AddOrUpdateAsync(remarkStatistics.Value);
        }

        private async Task HandleUserStatisticsAsync(RemarkDeleted @event)
        {
            var userStatistics = await _userStatisticsRepository.GetByIdAsync(@event.UserId);
            if (userStatistics.HasNoValue)
                return;

            userStatistics.Value.Remarks.IncreaseDeleted();
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
                categoryStats = new CategoryStatistics(remarkStats.Value.Category, new RemarksCountStatistics(reported: 1, deleted: 1));
            }

            categoryStats.Value.Remarks.IncreaseDeleted();
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
                    tagStats = new TagStatistics(tag, new RemarksCountStatistics(reported: 1, deleted: 1));
                }
                tagStats.Value.Remarks.IncreaseDeleted();
                await _tagStatisticsRepository.AddOrUpdateAsync(tagStats.Value);
            }
        }
    }
}