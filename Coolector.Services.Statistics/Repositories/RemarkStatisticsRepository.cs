using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Common.Mongo;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories.Queries;
using Coolector.Services.Statistics.Queries;
using MongoDB.Driver;


namespace Coolector.Services.Statistics.Repositories
{
    public class RemarkStatisticsRepository : IRemarkStatisticsRepository
    {
        private readonly IMongoDatabase _database;

        public RemarkStatisticsRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<PagedResult<RemarkStatistics>>> BrowseAsync(BrowseRemarkStatistics query)
            => await _database.RemarkStatistics()
                    .Query(query)
                    .PaginateAsync();

        public async Task<Maybe<RemarkGeneralStatistics>> GetGeneralStatisticsAsync(GetRemarkGeneralStatistics query)
            => await _database.RemarkStatistics()
                .CalculateGeneralStatisticsAsync(query);

        public async Task<Maybe<RemarkStatistics>> GetAsync(Guid remarkId)
            => await _database.RemarkStatistics().GetAsync(remarkId);

        public async Task AddOrUpdateAsync(RemarkStatistics statistics)
            => await _database.RemarkStatistics().AddOrUpdateAsync(statistics);
    }
}