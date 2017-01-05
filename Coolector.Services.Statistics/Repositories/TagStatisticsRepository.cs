using System.Threading.Tasks;
using Coolector.Common.Mongo;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Queries;
using Coolector.Services.Statistics.Repositories.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Statistics.Repositories
{
    public class TagStatisticsRepository : ITagStatisticsRepository
    {
        private readonly IMongoDatabase _database;

        public TagStatisticsRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<PagedResult<TagStatistics>>> BrowseAsync(BrowseTagStatistics query)
            => await _database.TagStatistics()
                .Query(query)
                .PaginateAsync();

        public async Task<Maybe<TagStatistics>> GetByNameAsync(string name)
            => await _database.TagStatistics()
                .GetByNameAsync(name);

        public async Task AddOrUpdateAsync(TagStatistics statistics)
            => await _database.TagStatistics()
                .AddOrUpdateAsync(statistics);
    }
}