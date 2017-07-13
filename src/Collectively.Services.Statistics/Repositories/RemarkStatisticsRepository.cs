using System;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Types;
using Collectively.Common.Mongo;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Repositories.Queries;
using Collectively.Services.Statistics.Queries;
using MongoDB.Driver;


namespace Collectively.Services.Statistics.Repositories
{
    public class RemarkStatisticsRepository : IRemarkStatisticsRepository
    {
        private readonly IMongoDatabase _database;

        public RemarkStatisticsRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<PagedResult<RemarkStatistics>>> BrowseAsync(BrowseRemarkStatistics query)
            => await _database.RemarkStatistics()
                    .Query(query)
                    .PaginateAsync();

        public async Task<Maybe<RemarksCountStatistics>> GetGeneralStatisticsAsync(GetRemarkGeneralStatistics query)
            => await _database.RemarkStatistics()
                .CalculateGeneralStatisticsAsync(query);

        public async Task<Maybe<RemarkStatistics>> GetAsync(Guid remarkId)
            => await _database.RemarkStatistics().GetAsync(remarkId);

        public async Task AddOrUpdateAsync(RemarkStatistics statistics)
            => await _database.RemarkStatistics().AddOrUpdateAsync(statistics);

        public async Task<Maybe<PagedResult<Vote>>> BrowseVotesAsync(BrowseRemarkVotes query)
        {
            var remarkStats = await GetAsync(query.RemarkId);
            if (remarkStats.HasNoValue || remarkStats.Value.Votes == null)
            {
                return PagedResult<Vote>.Empty;
            }

            return remarkStats.Value.Votes.Paginate(query);
        }

        public async Task AddVoteAsync(Vote vote)
        {
            var remarkStats = await _database.RemarkStatistics()
                .GetAsync(vote.RemarkId);

            remarkStats.AddVote(vote);
            await AddOrUpdateAsync(remarkStats);
        }
    }
}