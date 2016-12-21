namespace Coolector.Services.Statistics.Shared.Dto
{
    public class ReporterDto : IStatisticsDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}