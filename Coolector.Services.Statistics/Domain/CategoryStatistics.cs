namespace Coolector.Services.Statistics.Domain
{
    public class CategoryStatistics : StatisticsBase
    {
        protected CategoryStatistics() { }

        public CategoryStatistics(string name, uint reported = 0, uint resolved = 0, uint deleted = 0)
            :base(name, reported, resolved, deleted) { }
    }
}