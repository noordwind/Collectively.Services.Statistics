namespace Coolector.Services.Statistics.Shared.Dto
{
    public class UserStatisticsDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ReportedCount { get; set; }
        public string ResolvedCount { get; set; }
        public string DeletedCount { get; set; }
    }
}