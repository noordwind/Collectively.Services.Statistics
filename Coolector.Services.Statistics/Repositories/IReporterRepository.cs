using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Queries;

namespace Coolector.Services.Statistics.Repositories
{
    public interface IReporterRepository
    {
        Task<Maybe<PagedResult<Reporter>>> BrowseAsync(BrowseReporters query);
        Task<Maybe<Reporter>> GetByIdAsync(string userId);
        Task<Maybe<Reporter>> GetByNameAsync(string name);
        Task UpsertAsync(Reporter reporter);
    }
}