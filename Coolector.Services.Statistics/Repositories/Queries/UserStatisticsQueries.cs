using System.Threading.Tasks;
using Coolector.Services.Statistics.Domain;
using Coolector.Common.Mongo;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Statistics.Repositories.Queries
{
    public static class UserStatisticsQueries
    {
        public static IMongoCollection<UserStatistics> UserStatistics(this IMongoDatabase database)
            => database.GetCollection<UserStatistics>();

        public static async Task<UserStatistics> GetByIdAsync(this IMongoCollection<UserStatistics> userStatistics, string userId)
            => await userStatistics.AsQueryable().FirstOrDefaultAsync(x => x.UserId == userId);

        public static async Task<UserStatistics> GetByNameAsync(this IMongoCollection<UserStatistics> userStatistics, string name)
            => await userStatistics.AsQueryable().FirstOrDefaultAsync(x => x.Name == name);

        public static async Task AddOrUpdateAsync(this IMongoCollection<UserStatistics> userStatistics, UserStatistics user)
            => await userStatistics.ReplaceOneAsync(x => x.Id == user.Id, user, new UpdateOptions
            {
                IsUpsert = true
            });

        public static IMongoQueryable<UserStatistics> Query(this IMongoCollection<UserStatistics> userStatistics, BrowseUserStatistics query)
        {
            var values = userStatistics.AsQueryable();
            switch (query.OrderBy?.ToLowerInvariant())
            {
                case "reported":
                    values = values.OrderByDescending(x => x.ReportedCount);
                    break;
                case "resolved":
                    values = values.OrderByDescending(x => x.ResolvedCount);
                    break;
            }

            return values;
        }
    }
}