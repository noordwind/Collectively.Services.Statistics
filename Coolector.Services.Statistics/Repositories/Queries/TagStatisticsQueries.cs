using System.Threading.Tasks;
using Coolector.Common.Mongo;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Statistics.Repositories.Queries
{
    public static class TagStatisticsQueries
    {
        public static IMongoCollection<TagStatistics> TagStatistics(this IMongoDatabase database)
            => database.GetCollection<TagStatistics>();

        public static async Task<TagStatistics> GetByNameAsync(
            this IMongoCollection<TagStatistics> collection, string name)
        {
            return await collection.FirstOrDefaultAsync(x => x.Name == name);
        }

        public static async Task AddOrUpdateAsync(this IMongoCollection<TagStatistics> collection,
            TagStatistics statistics)
        {
            await collection.ReplaceOneAsync(x => x.Id == statistics.Id, statistics, new UpdateOptions
            {
                IsUpsert = true
            });
        }

        public static IMongoQueryable<TagStatistics> Query(
            this IMongoCollection<TagStatistics> collection,
            BrowseTagStatistics query)
        {
            var values = collection.AsQueryable();

            return values;
        }
    }
}