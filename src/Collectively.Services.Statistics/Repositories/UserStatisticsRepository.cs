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

        public async Task<Maybe<PagedResult<Vote>>> BrowseVotesAsync(BrowseUserVotes query)
        {
            var userStats = await GetByIdAsync(query.UserId);
            if (userStats.HasNoValue || userStats.Value.Votes == null)
            {
                return PagedResult<Vote>.Empty;
            }

            return userStats.Value.Votes.Paginate(query);
        }

        public async Task AddVoteAsync(Vote vote)
        {
            var userStats = await _database.UserStatistics()
                .GetByIdAsync(vote.UserId);

            userStats.AddVote(vote);
            await AddOrUpdateAsync(userStats);
        }
    }
}