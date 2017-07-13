using System;
using System.Threading.Tasks;
using Collectively.Services.Statistics.Domain;
using Collectively.Common.Mongo;
using Collectively.Services.Statistics.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;

namespace Collectively.Services.Statistics.Repositories.Queries
{
    public static class RemarkStatisticsQueries
    {
        public static IMongoCollection<RemarkStatistics> RemarkStatistics(this IMongoDatabase database)
            => database.GetCollection<RemarkStatistics>();

        public static async Task<RemarkStatistics> GetAsync(this IMongoCollection<RemarkStatistics> statistics, Guid remarkId)
            => await statistics.AsQueryable().FirstOrDefaultAsync(x => x.RemarkId == remarkId);

        public static async Task AddOrUpdateAsync(this IMongoCollection<RemarkStatistics> statistics, RemarkStatistics value)
            => await statistics.ReplaceOneAsync(x => x.Id == value.Id, value, new UpdateOptions
            {
                IsUpsert = true
            });

        public static IMongoQueryable<RemarkStatistics> Query(this IMongoCollection<RemarkStatistics> statistics, BrowseRemarkStatistics query)
        {
            var values = statistics.AsQueryable();

            return values;
        }

        public static async Task<RemarksCountStatistics> CalculateGeneralStatisticsAsync(
            this IMongoCollection<RemarkStatistics> statistics,
            GetRemarkGeneralStatistics query)
        {
            var from = query.From.GetValueOrDefault(default(DateTime));
            var to = query.To.GetValueOrDefault(DateTime.MaxValue);
            var states = await statistics.AsQueryable()
                .Where(x => x.State.CreatedAt > from && x.State.CreatedAt < to)
                .Select(x => x.State.State)
                .ToListAsync();

            var reported = states.Count();
            var @new = states.Count(x => x == RemarkState.Names.New);
            var processing = states.Count(x => x == RemarkState.Names.Processing);
            var resolved = states.Count(x => x == RemarkState.Names.Resolved);
            var canceled = states.Count(x => x == RemarkState.Names.Canceled);
            var renewed = states.Count(x => x == RemarkState.Names.Renewed);
            var deleted = reported - @new - processing - resolved - canceled - renewed;

            return new RemarksCountStatistics(@new, reported, processing,
                resolved, canceled, deleted, renewed);
        }
    }
}