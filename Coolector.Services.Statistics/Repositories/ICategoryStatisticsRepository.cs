using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Queries;

namespace Coolector.Services.Statistics.Repositories
{
    public interface ICategoryStatisticsRepository
    {
        Task<Maybe<PagedResult<CategoryStatistics>>> BrowseAsync(BrowseCategoryStatistics query);
        Task<Maybe<CategoryStatistics>> GetByNameAsync(string name);
        Task AddOrUpdateAsync(CategoryStatistics statistics);
    }
}