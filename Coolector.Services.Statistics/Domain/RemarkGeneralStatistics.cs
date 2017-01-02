using Autofac.Core;

namespace Coolector.Services.Statistics.Domain
{
    public class RemarkGeneralStatistics
    {
        public int Reported { get; }
        public int Resolved { get; }

        public RemarkGeneralStatistics(int reported, int resolved)
        {
            Reported = reported;
            Resolved = resolved;
        }
    }
}