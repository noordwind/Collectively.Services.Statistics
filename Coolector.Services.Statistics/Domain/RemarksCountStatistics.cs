namespace Coolector.Services.Statistics.Domain
{
    public class RemarksCountStatistics : RemarksCountStatisticsBase
    {
        protected RemarksCountStatistics() { }

        public RemarksCountStatistics(int @new = 0, int reported = 0, 
                int processing = 0, int resolved = 0, int canceled = 0, 
                int deleted = 0, int renewed = 0)
            : base(@new, reported, processing, resolved,
                canceled, deleted, renewed) { }
    }
}