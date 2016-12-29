namespace Coolector.Services.Statistics.Shared.Dto
{
    public class UserStatisticsDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public int ReportedCount { get; set; }
        public int ResolvedCount { get; set; }
        public int DeletedCount { get; set; }
    }
}