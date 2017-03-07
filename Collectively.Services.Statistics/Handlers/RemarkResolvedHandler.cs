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
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IHandler _handler;
        private readonly IRemarkStatisticsRepository _remarkStatisticsRepository;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly ICategoryStatisticsRepository _categoryStatisticsRepository;
        private readonly ITagStatisticsRepository _tagStatisticsRepository;
        private readonly IServiceClient _serviceClient;

        public RemarkResolvedHandler(IHandler handler, 
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

            var remark = await _serviceClient.GetAsync<RemarkDto>(@event.Resource);
            Location location = null;
            if (remark.Value.State.Location != null)
            {
                location = new Location(remark.Value.State.Location.Latitude,
                    remark.Value.Location.Longitude, remark.Value.Location.Address);
            }

            remarkStatistics.Value.AddState(new RemarkState(remark.Value.State.State, @event.UserId, location: location));
            await _remarkStatisticsRepository.AddOrUpdateAsync(remarkStatistics.Value);
        }

        private async Task HandleUserStatisticsAsync(RemarkResolved @event)
        {
            var userStatistics = await _userStatisticsRepository.GetByIdAsync(@event.UserId);
            var remark = await _serviceClient.GetAsync<RemarkDto>(@event.Resource);
            if (userStatistics.HasNoValue)
            {
                userStatistics = new UserStatistics(@event.UserId, remark.Value.State.User.Name,
                    new RemarksCountStatistics(reported: 1, resolved: 1));
            }

            userStatistics.Value.Remarks.IncreaseResolved();
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
                categoryStats = new CategoryStatistics(remarkStats.Value.Category, 
                    new RemarksCountStatistics(reported: 1, resolved: 1));
            }

            categoryStats.Value.Remarks.IncreaseResolved();
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
                    tagStats = new TagStatistics(tag, new RemarksCountStatistics(reported: 1, resolved: 1));
                }
                tagStats.Value.Remarks.IncreaseResolved();
                await _tagStatisticsRepository.AddOrUpdateAsync(tagStats.Value);
            }
        }
    }
}