using Coolector.Common.Queries;

namespace Coolector.Services.Statistics.Shared.Queries
{
    public class BrowseReporters : IPagedQuery
    {
        public int Page { get; set; }
        public int Results { get; set; }
    }
}