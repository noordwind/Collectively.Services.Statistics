using System;
using Coolector.Common.Extensions;

namespace Coolector.Services.Statistics.Domain
{
    public class TagStatistics : RemarksCountStatisticsBase
    {
        public string Name { get; protected set; }

        protected TagStatistics() { }

        public TagStatistics(string name, int @new = 0, int reported = 0, 
                int processing = 0, int resolved = 0, int canceled = 0, 
                int deleted = 0, int renewed = 0)
            : base(@new, reported, processing, resolved,
                canceled, deleted, renewed) 
        {
            if (name.Empty())
            {
                throw new ArgumentException("Tag name can not be empty.", nameof(name));
            }
            Name = name;
        }
    }
}