using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Queries;

namespace Collectively.Services.Statistics.Repositories
{
    public interface IRemarkStatisticsRepository
    {
        Task<Maybe<PagedResult<RemarkStatistics>>> BrowseAsync(BrowseRemarkStatistics query);
        Task<Maybe<RemarkStatistics>> GetAsync(Guid remarkId);
        Task<Maybe<RemarksCountStatistics>> GetGeneralStatisticsAsync(GetRemarkGeneralStatistics query);
        Task AddOrUpdateAsync(RemarkStatistics statistics);
        Task<Maybe<PagedResult<Vote>>> BrowseVotesAsync(BrowseRemarkVotes query);
        Task AddVoteAsync(Vote vote);
    }
}