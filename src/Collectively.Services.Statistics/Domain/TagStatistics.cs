using System;
using Collectively.Common.Extensions;

namespace Collectively.Services.Statistics.Domain
{
    public class TagStatistics
    {
        public RemarkTag Tag { get; protected set; }

        public RemarksCountStatistics Remarks { get; protected set; }

        protected TagStatistics() { }

        public TagStatistics(RemarkTag tag, RemarksCountStatistics remarks = null)
        {
            if (tag == null)
            {
                throw new ArgumentException("Remark tag can not be null.", nameof(tag));
            }
            Tag = tag;
            Remarks = remarks ?? new RemarksCountStatistics();
        }
    }
}