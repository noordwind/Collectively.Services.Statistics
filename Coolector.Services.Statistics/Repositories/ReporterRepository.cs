using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Queries;
using Coolector.Services.Statistics.Repositories.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Statistics.Repositories
{
    public class ReporterRepository : IReporterRepository
    {
        private readonly IMongoDatabase _database;

        public ReporterRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<PagedResult<Reporter>>> BrowseAsync(BrowseReporters query)
            => await _database.Reporters().QueryAsync(query);

        public async Task<Maybe<Reporter>> GetByIdAsync(string userId)
            => await _database.Reporters().GetByIdAsync(userId);

        public async Task<Maybe<Reporter>> GetByNameAsync(string name)
            => await _database.Reporters().GetByNameAsync(name);

        public async Task UpsertAsync(Reporter reporter)
            => await _database.Reporters().UpsertAsync(reporter);
    }
}