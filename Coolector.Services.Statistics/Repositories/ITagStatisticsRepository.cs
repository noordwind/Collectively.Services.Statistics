using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Queries;

namespace Coolector.Services.Statistics.Repositories
{
    public interface ITagStatisticsRepository
    {
        Task<Maybe<PagedResult<TagStatistics>>> BrowseAsync(BrowseTagStatistics query);
        Task<Maybe<TagStatistics>> GetByNameAsync(string name);
        Task AddOrUpdateAsync(TagStatistics statistics);
    }
}