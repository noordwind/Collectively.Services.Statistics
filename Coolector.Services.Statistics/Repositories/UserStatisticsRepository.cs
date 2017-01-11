using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Coolector.Common.Mongo;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories.Queries;
using Coolector.Services.Statistics.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Statistics.Repositories
{
    public class UserStatisticsRepository : IUserStatisticsRepository
    {
        private readonly IMongoDatabase _database;

        public UserStatisticsRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<PagedResult<UserStatistics>>> BrowseAsync(BrowseUserStatistics query)
            => await _database.UserStatistics()
                    .Query(query)
                    .PaginateAsync();

        public async Task<Maybe<UserStatistics>> GetByIdAsync(string userId)
            => await _database.UserStatistics().GetByIdAsync(userId);

        public async Task<Maybe<UserStatistics>> GetByNameAsync(string name)
            => await _database.UserStatistics().GetByNameAsync(name);

        public async Task AddOrUpdateAsync(UserStatistics statistics)
            => await _database.UserStatistics().AddOrUpdateAsync(statistics);

        public async Task<Maybe<PagedResult<VoteStatistics>>> BrowseVotesAsync(BrowseUserVotes query)
        {
            var userStats = await GetByIdAsync(query.UserId);
            if (userStats.HasNoValue || userStats.Value.Votes == null)
            {
                return PagedResult<VoteStatistics>.Empty;
            }

            return userStats.Value.Votes.Paginate(query);
        }

        public async Task AddVoteAsync(VoteStatistics vote)
        {
            var userStats = await _database.UserStatistics()
                .GetByIdAsync(vote.UserId);

            userStats.AddVote(vote);
            await AddOrUpdateAsync(userStats);
        }
    }
}