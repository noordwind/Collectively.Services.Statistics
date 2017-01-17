namespace Coolector.Services.Statistics.Domain
{
    public class TagStatistics : StatisticsBase
    {
        protected TagStatistics() { }

        public TagStatistics(string name, uint reported = 0, uint resolved = 0, uint deleted = 0)
            :base(name, reported, resolved, deleted) { }
    }
}