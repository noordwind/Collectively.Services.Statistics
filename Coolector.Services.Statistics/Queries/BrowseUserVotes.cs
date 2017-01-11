using Coolector.Common.Types;

namespace Coolector.Services.Statistics.Queries
{
    public class BrowseUserVotes : PagedQueryBase
    {
        public string UserId { get; set; }
    }
}