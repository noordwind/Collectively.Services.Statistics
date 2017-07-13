using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Repositories;
using Collectively.Services.Statistics.Dto;
using Collectively.Common.ServiceClients;
using System;

namespace Collectively.Services.Statistics.Handlers
{
    public abstract class RemarkStateChangedBaseHandler<T> : IEventHandler<T> where T : RemarkStateChangedBase
    {
        private readonly IHandler _handler;
        private readonly IRemarkStatisticsRepository _remarkStatisticsRepository;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly ICategoryStatisticsRepository _categoryStatisticsRepository;
        private readonly ITagStatisticsRepository _tagStatisticsRepository;
        private readonly IServiceClient _serviceClient;
        private Action<UserStatistics> _updateUser;
        private Action<CategoryStatistics> _updateCategory;
        private Action<TagStatistics> _updateTags;
        

        public RemarkStateChangedBaseHandler(IHandler handler, 
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

        protected void Setup(Action<UserStatistics> updateUser, 
            Action<CategoryStatistics> updateCategory, 
            Action<TagStatistics> updateTags)
        {
            _updateUser = updateUser;
            _updateCategory = updateCategory;
            _updateTags = updateTags;
        }

        public async Task HandleAsync(T @event)
        {
            await _handler
                .Run(async () =>
                {
                    await HandleRemarkStatisticsAsync(@event);
                    await HandleUserStatisticsAsync(@event);
                    await HandleCategoryStatisticsAsync(@event);
                    await HandleTagStatisticsAsync(@event);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {typeof(T).Name} event"))
                .ExecuteAsync();
        }

        private async Task HandleRemarkStatisticsAsync(T @event)
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
            else
            {
                Location location = null;
                if (remark.Value.State.Location != null)
                {
                    location = new Location(remark.Value.State.Location.Latitude,
                        remark.Value.Location.Longitude, remark.Value.Location.Address);
                }
                remarkStatistics.Value.AddState(new RemarkState(remark.Value.State.State, @event.UserId, location: location));
            }
            await _remarkStatisticsRepository.AddOrUpdateAsync(remarkStatistics.Value);
        }

        private async Task HandleUserStatisticsAsync(T @event)
        {
            var userStatistics = await _userStatisticsRepository.GetByIdAsync(@event.UserId);
            var remark = await _serviceClient.GetAsync<RemarkDto>(@event.Resource);
            if (userStatistics.HasNoValue)
            {
                userStatistics = new UserStatistics(@event.UserId, remark.Value.State.User.Name,
                    new RemarksCountStatistics());
            }
            _updateUser(userStatistics.Value);
            await _userStatisticsRepository.AddOrUpdateAsync(userStatistics.Value);
        }

        private async Task HandleCategoryStatisticsAsync(T @event)
        {
            var remarkStats = await _remarkStatisticsRepository.GetAsync(@event.RemarkId);
            if (remarkStats.HasNoValue)
                return;

            var categoryStats = await _categoryStatisticsRepository.GetByNameAsync(remarkStats.Value.Category);
            if (categoryStats.HasNoValue)
            {
                categoryStats = new CategoryStatistics(remarkStats.Value.Category, 
                    new RemarksCountStatistics());
            }
            _updateCategory(categoryStats.Value);
            await _categoryStatisticsRepository.AddOrUpdateAsync(categoryStats.Value);
        }

        private async Task HandleTagStatisticsAsync(T @event)
        {
            var remarkStats = await _remarkStatisticsRepository.GetAsync(@event.RemarkId);
            if (remarkStats.HasNoValue)
                return;

            foreach (var tag in remarkStats.Value.Tags)
            {
                var tagStats = await _tagStatisticsRepository.GetByNameAsync(tag);
                if (tagStats.HasNoValue)
                {
                    tagStats = new TagStatistics(tag, new RemarksCountStatistics());
                }
                _updateTags(tagStats.Value);
                await _tagStatisticsRepository.AddOrUpdateAsync(tagStats.Value);
            }
        }
    }
}