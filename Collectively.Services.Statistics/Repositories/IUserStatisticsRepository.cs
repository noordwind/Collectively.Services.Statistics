using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Queries;

namespace Collectively.Services.Statistics.Repositories
{
    public interface IUserStatisticsRepository
    {
        Task<Maybe<PagedResult<UserStatistics>>> BrowseAsync(BrowseUserStatistics query);
        Task<Maybe<UserStatistics>> GetByIdAsync(string userId);
        Task<Maybe<UserStatistics>> GetByNameAsync(string name);
        Task AddOrUpdateAsync(UserStatistics statistics);
        Task<Maybe<PagedResult<Vote>>> BrowseVotesAsync(BrowseUserVotes query);
        Task AddVoteAsync(Vote vote);
    }
}