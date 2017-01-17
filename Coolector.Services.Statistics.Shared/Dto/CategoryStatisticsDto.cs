namespace Coolector.Services.Statistics.Shared.Dto
{
    public class CategoryStatisticsDto
    {
        public string Name { get; set; }
        public int ReportedCount { get; set; }
        public int ResolvedCount { get; set; }
        public int DeletedCount { get; set; }
    }
}