using System.Threading.Tasks;
using Coolector.Common.Mongo;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Shared.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Statistics.Repositories.Queries
{
    public static class ResolverQueries
    {
        public static IMongoCollection<Resolver> Resolvers(this IMongoDatabase database)
            => database.GetCollection<Resolver>();

        public static async Task<Resolver> GetByNameAsync(this IMongoCollection<Resolver> resolvers, string name)
            => await resolvers.AsQueryable().FirstOrDefaultAsync(x => x.User.Name == name);

        public static async Task UpsertAsync(this IMongoCollection<Resolver> resolvers, Resolver resolver)
            => await resolvers.ReplaceOneAsync(x => x.Id == resolver.Id, resolver, new UpdateOptions
            {
                IsUpsert = true
            });

        public static async Task<PagedResult<Resolver>> QueryAsync(this IMongoCollection<Resolver> resolvers, BrowseResolvers query)
        {
            var totalCount = await resolvers.AsQueryable().CountAsync();
            var totalPages = (int)totalCount / query.Results + 1;
            var values = await resolvers
                .AsQueryable()
                .OrderByDescending(x => x.ResolvedCount)
                .Limit(query.Page, query.Results)
                .ToListAsync();

            return PagedResult<Resolver>.Create(values, query.Page, query.Results, totalPages, totalCount);
        }
    }
}