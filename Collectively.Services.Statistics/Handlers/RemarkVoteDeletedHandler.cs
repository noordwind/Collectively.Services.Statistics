using System.Linq;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Queries;
using Collectively.Services.Statistics.Repositories;
using Collectively.Common.ServiceClients.Remarks;

namespace Collectively.Services.Statistics.Handlers
{
    public class RemarkVoteDeletedHandler : IEventHandler<RemarkVoteDeleted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkStatisticsRepository _remarkStatisticsRepository;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly IRemarkServiceClient _remarkServiceClient;

        public RemarkVoteDeletedHandler(IHandler handler, 
            IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository,
            IRemarkServiceClient remarkServiceClient)
        {
            _handler = handler;
            _remarkStatisticsRepository = remarkStatisticsRepository;
            _userStatisticsRepository = userStatisticsRepository;
            _remarkServiceClient = remarkServiceClient;
        }

        public async Task HandleAsync(RemarkVoteDeleted @event)
        {
            await _handler
                .Run(async () =>
                {
                    var votes = await _remarkStatisticsRepository.BrowseVotesAsync(new BrowseRemarkVotes
                    {
                        Page = 1,
                        Results = int.MaxValue,
                        RemarkId = @event.RemarkId
                    });
                    if (votes.HasValue)
                    {
                        var originalVote = votes.Value.Items
                            .LastOrDefault(x => x.UserId == @event.UserId);
                        var vote = Vote.CreateDeletedVote(originalVote);
                        await _remarkStatisticsRepository.AddVoteAsync(vote);
                    }
                })
                .OnError((ex, logger) => logger.Debug(ex, 
                    $"Error while handling {@event.GetType().Name} event in remark statistics"))
                .Next()
                .Run(async () =>
                {
                    var votes = await _userStatisticsRepository.BrowseVotesAsync(new BrowseUserVotes
                    {
                        Page = 1,
                        Results = int.MaxValue,
                        UserId = @event.UserId
                    });
                    if (votes.HasValue)
                    {
                        var originalVote = votes.Value.Items
                            .LastOrDefault(x => x.RemarkId == @event.RemarkId);
                        var vote = Vote.CreateDeletedVote(originalVote);
                        await _userStatisticsRepository.AddVoteAsync(vote);
                    }
                })
                .OnError((ex, logger) => logger.Debug(ex, 
                    $"Error while handling {@event.GetType().Name} event in user statistics"))
                .Next()
                .ExecuteAllAsync();

        }
    }
}