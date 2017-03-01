using System.Threading.Tasks;
using Collectively.Services.Statistics.Domain;
using Collectively.Common.Mongo;
using Collectively.Services.Statistics.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Statistics.Repositories.Queries
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
            => await userStatistics.ReplaceOneAsync(x => x.UserId == user.UserId, user, new UpdateOptions
            {
                IsUpsert = true
            });

        public static IMongoQueryable<UserStatistics> Query(this IMongoCollection<UserStatistics> userStatistics, BrowseUserStatistics query)
        {
            var values = userStatistics.AsQueryable();
            switch (query.OrderBy?.ToLowerInvariant())
            {
                case "reported":
                    values = values.OrderByDescending(x => x.Remarks.ReportedCount);
                    break;
                case "resolved":
                    values = values.OrderByDescending(x => x.Remarks.ResolvedCount);
                    break;
            }

            return values;
        }
    }
}