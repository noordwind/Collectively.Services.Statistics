using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Queries;

namespace Coolector.Services.Statistics.Repositories
{
    public interface IRemarkStatisticsRepository
    {
        Task<Maybe<PagedResult<RemarkStatistics>>> BrowseAsync(BrowseRemarkStatistics query);
        Task<Maybe<RemarkStatistics>> GetAsync(Guid remarkId);
        Task<Maybe<RemarkGeneralStatistics>> GetGeneralStatisticsAsync(GetRemarkGeneralStatistics query);
        Task UpsertAsync(RemarkStatistics statistics);
    }
}