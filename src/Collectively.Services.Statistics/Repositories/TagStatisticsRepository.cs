using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Types;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Queries;
using Collectively.Services.Statistics.Repositories.Queries;
using MongoDB.Driver;

namespace Collectively.Services.Statistics.Repositories
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
                .PaginateAsync(query);

        public async Task<Maybe<TagStatistics>> GetByDefaultIdAsync(Guid id)
            => await _database.TagStatistics()
                .GetByDefaultIdAsync(id);

        public async Task AddOrUpdateAsync(TagStatistics statistics)
            => await _database.TagStatistics()
                .AddOrUpdateAsync(statistics);
    }
}