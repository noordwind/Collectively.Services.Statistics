using System.Threading.Tasks;
using Coolector.Services.Statistics.Domain;
using Coolector.Common.Mongo;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Statistics.Repositories.Queries
{
    public static class ReporterQueries
    {
        public static IMongoCollection<Reporter> Reporters(this IMongoDatabase database)
            => database.GetCollection<Reporter>();

        public static async Task<Reporter> GetByIdAsync(this IMongoCollection<Reporter> reporters, string userId)
            => await reporters.AsQueryable().FirstOrDefaultAsync(x => x.User.UserId == userId);

        public static async Task<Reporter> GetByNameAsync(this IMongoCollection<Reporter> reporters, string name)
            => await reporters.AsQueryable().FirstOrDefaultAsync(x => x.User.Name == name);

        public static async Task UpsertAsync(this IMongoCollection<Reporter> reporters, Reporter reporter)
            => await reporters.ReplaceOneAsync(x => x.Id == reporter.Id, reporter, new UpdateOptions
            {
                IsUpsert = true
            });

        public static async Task<PagedResult<Reporter>> QueryAsync(this IMongoCollection<Reporter> reporters, BrowseReporters query)
        {
            var totalCount = await reporters.AsQueryable().CountAsync();
            var totalPages = (int)totalCount / query.Results + 1;
            var values = await reporters
                .AsQueryable()
                .OrderByDescending(x => x.ReportedCount)
                .Skip(query.Results * (query.Page - 1))
                .Limit(query.Results)
                .ToListAsync();

            return PagedResult<Reporter>.Create(values, query.Page, query.Results, totalPages, totalCount);
        }
    }
}