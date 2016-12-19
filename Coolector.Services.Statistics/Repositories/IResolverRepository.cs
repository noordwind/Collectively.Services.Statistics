using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Queries;

namespace Coolector.Services.Statistics.Repositories
{
    public interface IResolverRepository
    {
        Task<Maybe<PagedResult<Resolver>>> BrowseAsync(BrowseResolvers query);
        Task<Maybe<Resolver>> GetByNameAsync(string name);
        Task UpsertAsync(Resolver resolver);
    }
}