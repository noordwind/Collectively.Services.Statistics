using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Repositories;
using Collectively.Common.ServiceClients.Remarks;

namespace Collectively.Services.Statistics.Handlers
{
    public class RemarkVoteSubmittedHandler : IEventHandler<RemarkVoteSubmitted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkStatisticsRepository _remarkStatisticsRepository;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly IRemarkServiceClient _remarkServiceClient;

        public RemarkVoteSubmittedHandler(IHandler handler, 
            IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository,
            IRemarkServiceClient remarkServiceClient)
        {
            _handler = handler;
            _remarkStatisticsRepository = remarkStatisticsRepository;
            _userStatisticsRepository = userStatisticsRepository;
            _remarkServiceClient = remarkServiceClient;
        }

        public async Task HandleAsync(RemarkVoteSubmitted @event)
        {
            await _handler
                .Run(async () =>
                {
                    var vote = @event.Positive
                        ? Vote.CreatePositiveVote(@event.UserId, @event.RemarkId)
                        : Vote.CreateNegativeVote(@event.UserId, @event.RemarkId);
                    await _remarkStatisticsRepository.AddVoteAsync(vote);
                    await _userStatisticsRepository.AddVoteAsync(vote);
                })
                .OnError((ex, logger) => logger.Debug(ex, $"Error while handling {@event.GetType().Name} event"))
                .ExecuteAsync();
        }
    }
}