using System;
using Coolector.Common.Types;

namespace Coolector.Services.Statistics.Queries
{
    public class BrowseRemarkVotes : PagedQueryBase
    {
        public Guid RemarkId { get; set; }
    }
}