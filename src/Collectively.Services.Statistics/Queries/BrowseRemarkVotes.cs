using System;
using Collectively.Common.Types;

namespace Collectively.Services.Statistics.Queries
{
    public class BrowseRemarkVotes : PagedQueryBase
    {
        public Guid RemarkId { get; set; }
    }
}