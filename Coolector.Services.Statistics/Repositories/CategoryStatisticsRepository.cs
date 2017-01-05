using System.Threading.Tasks;
using Coolector.Common.Mongo;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Queries;
using Coolector.Services.Statistics.Repositories.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Statistics.Repositories
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
                .PaginateAsync();

        public async Task<Maybe<CategoryStatistics>> GetByNameAsync(string name)
            => await _database.CategoryStatistics()
                .GetByNameAsync(name);

        public async Task AddOrUpdateAsync(CategoryStatistics statistics)
            => await _database.CategoryStatistics()
                .AddOrUpdateAsync(statistics);
    }
}