using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories.Queries;
using Coolector.Services.Statistics.Shared.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Statistics.Repositories
{
    public class ResolverRepository : IResolverRepository
    {
        private readonly IMongoDatabase _database;

        public ResolverRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<PagedResult<Resolver>>> BrowseAsync(BrowseResolvers query)
            => await _database.Resolvers().QueryAsync(query);

        public async Task<Maybe<Resolver>> GetByNameAsync(string name)
            => await _database.Resolvers().GetByNameAsync(name);

        public async Task UpsertAsync(Resolver resolver)
            => await _database.Resolvers().UpsertAsync(resolver);
    }
}