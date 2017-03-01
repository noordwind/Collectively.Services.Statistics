using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Repositories.Queries;
using MongoDB.Driver;

namespace Collectively.Services.Statistics.Framework
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
                var reportedIndex = new IndexKeysDefinitionBuilder<UserStatistics>().Descending(x => x.Remarks.ReportedCount);
                var resolvedIndex = new IndexKeysDefinitionBuilder<UserStatistics>().Descending(x => x.Remarks.ResolvedCount);
                await _database.UserStatistics().Indexes.CreateOneAsync(reportedIndex);
                await _database.UserStatistics().Indexes.CreateOneAsync(resolvedIndex);
            }
        }
    }
}