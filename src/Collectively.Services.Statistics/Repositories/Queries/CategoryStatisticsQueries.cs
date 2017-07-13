using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Statistics.Repositories.Queries
{
    public static class CategoryStatisticsQueries
    {
        public static IMongoCollection<CategoryStatistics> CategoryStatistics(this IMongoDatabase database)
            => database.GetCollection<CategoryStatistics>();

        public static async Task<CategoryStatistics> GetByNameAsync(
            this IMongoCollection<CategoryStatistics> collection, string name)
        {
            return await collection.FirstOrDefaultAsync(x => x.Name == name);
        }

        public static async Task AddOrUpdateAsync(this IMongoCollection<CategoryStatistics> collection,
            CategoryStatistics statistics)
        {
            await collection.ReplaceOneAsync(x => x.Name == statistics.Name, statistics, new UpdateOptions
            {
                IsUpsert = true
            });
        }

        public static IMongoQueryable<CategoryStatistics> Query(
            this IMongoCollection<CategoryStatistics> collection,
            BrowseCategoryStatistics query)
        {
            var values = collection
                .AsQueryable()
                .OrderByDescending(x => x.Remarks.ReportedCount);

            return values;
        }
    }
}