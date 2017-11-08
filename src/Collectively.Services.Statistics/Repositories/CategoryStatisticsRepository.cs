using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Types;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Queries;
using Collectively.Services.Statistics.Repositories.Queries;
using MongoDB.Driver;

namespace Collectively.Services.Statistics.Repositories
{
    public class CategoryStatisticsRepository : ICategoryStatisticsRepository
    {
        private readonly IMongoDatabase _database;

        public CategoryStatisticsRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<PagedResult<CategoryStatistics>>> BrowseAsync(BrowseCategoryStatistics query)
            => await _database.CategoryStatistics()
                .Query(query)
                .PaginateAsync(query);

        public async Task<Maybe<CategoryStatistics>> GetByNameAsync(string name)
            => await _database.CategoryStatistics()
                .GetByNameAsync(name);

        public async Task AddOrUpdateAsync(CategoryStatistics statistics)
            => await _database.CategoryStatistics()
                .AddOrUpdateAsync(statistics);
    }
}