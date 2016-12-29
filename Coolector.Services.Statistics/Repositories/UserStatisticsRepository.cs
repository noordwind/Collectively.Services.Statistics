using System.Threading.Tasks;
using Coolector.Common.Types;
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
            => await _database.UserStatistics().QueryAsync(query);

        public async Task<Maybe<UserStatistics>> GetByIdAsync(string userId)
            => await _database.UserStatistics().GetByIdAsync(userId);

        public async Task<Maybe<UserStatistics>> GetByNameAsync(string name)
            => await _database.UserStatistics().GetByNameAsync(name);

        public async Task UpsertAsync(UserStatistics statistics)
            => await _database.UserStatistics().UpsertAsync(statistics);
    }
}