using System.Threading.Tasks;
using Coolector.Common.Mongo;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Statistics.Framework
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IMongoDatabase _database;

        public DatabaseSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task SeedAsync()
        {
            if (await _database.Reporters().AsQueryable().AnyAsync() == false)
            {
                var index = new IndexKeysDefinitionBuilder<Reporter>().Descending(x => x.ReportedCount);
                await _database.Reporters().Indexes.CreateOneAsync(index);
            }
            if (await _database.Resolvers().AsQueryable().AnyAsync() == false)
            {
                var index = new IndexKeysDefinitionBuilder<Resolver>().Descending(x => x.ResolvedCount);
                await _database.Resolvers().Indexes.CreateOneAsync(index);
            }
        }
    }
}