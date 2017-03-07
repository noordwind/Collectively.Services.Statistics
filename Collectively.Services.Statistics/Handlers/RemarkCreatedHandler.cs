using System.Linq;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Repositories;
using Collectively.Services.Statistics.Dto;
using Collectively.Common.ServiceClients;

namespace Collectively.Services.Statistics.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private readonly IHandler _handler;
        private readonly IRemarkStatisticsRepository _remarkStatisticsRepository;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly ICategoryStatisticsRepository _categoryStatisticsRepository;
        private readonly ITagStatisticsRepository _tagStatisticsRepository;
        private readonly IServiceClient _serviceClient;

        public RemarkCreatedHandler(IHandler handler, 
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
            var remark = await _serviceClient.GetAsync<RemarkDto>(@event.Resource);
            if (remarkStatistics.HasNoValue)
            {
                remarkStatistics = new RemarkStatistics(@event.RemarkId,
                    remark.Value.Category.Name, @event.UserId, remark.Value.Author.Name,
                    remark.Value.CreatedAt, remark.Value.State.State, 
                    remark.Value.Location.Latitude, remark.Value.Location.Longitude, 
                    remark.Value.Location.Address, remark.Value.Description, remark.Value.Tags);
            }

            await _remarkStatisticsRepository.AddOrUpdateAsync(remarkStatistics.Value);
        }

        private async Task HandleUserStatisticsAsync(RemarkCreated @event)
        {
            var remark = await _serviceClient.GetAsync<RemarkDto>(@event.Resource);
            var userStatistics = await _userStatisticsRepository.GetByIdAsync(@event.UserId);
            if (userStatistics.HasNoValue)
            {
                userStatistics = new UserStatistics(@event.UserId, remark.Value.Author.Name,
                    new RemarksCountStatistics(@new: 1, reported: 1));
            }

            userStatistics.Value.Remarks.IncreaseReported();
            await _userStatisticsRepository.AddOrUpdateAsync(userStatistics.Value);
        }

        private async Task HandleCategoryStatisticsAsync(RemarkCreated @event)
        {
            var remark = await _serviceClient.GetAsync<RemarkDto>(@event.Resource);
            var categoryStatistics = await _categoryStatisticsRepository.GetByNameAsync(remark.Value.Category.Name);
            if (categoryStatistics.HasNoValue)
            {
                categoryStatistics = new CategoryStatistics(remark.Value.Category.Name, 
                    new RemarksCountStatistics(@new: 1, reported: 1));
            }

            categoryStatistics.Value.Remarks.IncreaseReported();
            await _categoryStatisticsRepository.AddOrUpdateAsync(categoryStatistics.Value);
        }

        private async Task HandleTagStatisticsAsync(RemarkCreated @event)
        {
            var remark = await _serviceClient.GetAsync<RemarkDto>(@event.Resource);
            if (@remark.Value.Tags == null || @remark.Value.Tags.Any() == false)
                return;

            foreach (var tag in @remark.Value.Tags)
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