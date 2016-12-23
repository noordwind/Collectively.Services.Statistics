using System.Collections.Generic;
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
            if (await _database.UserStatistics().AsQueryable().AnyAsync() == false)
            {
                var idIndex = new IndexKeysDefinitionBuilder<UserStatistics>().Ascending(x => x.UserId);
                await _database.UserStatistics().Indexes.CreateOneAsync(idIndex, new CreateIndexOptions
                {
                    Unique = true,
                });
                var reportedIndex = new IndexKeysDefinitionBuilder<UserStatistics>().Descending(x => x.ReportedCount);
                var resolvedIndex = new IndexKeysDefinitionBuilder<UserStatistics>().Descending(x => x.ResolvedCount);
                await _database.UserStatistics().Indexes.CreateOneAsync(reportedIndex);
                await _database.UserStatistics().Indexes.CreateOneAsync(resolvedIndex);
            }
        }
    }
}