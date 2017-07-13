using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Queries;

namespace Collectively.Services.Statistics.Repositories
{
    public interface ICategoryStatisticsRepository
    {
        Task<Maybe<PagedResult<CategoryStatistics>>> BrowseAsync(BrowseCategoryStatistics query);
        Task<Maybe<CategoryStatistics>> GetByNameAsync(string name);
        Task AddOrUpdateAsync(CategoryStatistics statistics);
    }
}