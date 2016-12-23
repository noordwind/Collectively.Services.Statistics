using System.Threading.Tasks;
using Coolector.Services.Statistics.Domain;
using Coolector.Common.Mongo;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Queries;
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

        public static async Task UpsertAsync(this IMongoCollection<UserStatistics> userStatistics, UserStatistics user)
            => await userStatistics.ReplaceOneAsync(x => x.Id == user.Id, user, new UpdateOptions
            {
                IsUpsert = true
            });

        public static async Task<PagedResult<UserStatistics>> QueryAsync(this IMongoCollection<UserStatistics> userStatistics, BrowseUserStatistics query)
        {
            var totalCount = await userStatistics.AsQueryable().CountAsync();
            var totalPages = (int)totalCount / query.Results + 1;
            var queryable = userStatistics.AsQueryable();
            switch (query.OrderBy)
            {
                case "reported":
                    queryable.OrderByDescending(x => x.ReportedCount);
                    break;
                case "resolved":
                    queryable.OrderByDescending(x => x.ResolvedCount);
                    break;
            }
            var values = await queryable
                .Limit(query.Page, query.Results)
                .ToListAsync();

            return PagedResult<UserStatistics>.Create(values, query.Page, query.Results, totalPages, totalCount);
        }
    }
}