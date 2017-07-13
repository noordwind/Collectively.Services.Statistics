using Collectively.Common.Types;

namespace Collectively.Services.Statistics.Queries
{
    public class BrowseUserVotes : PagedQueryBase
    {
        public string UserId { get; set; }
    }
}