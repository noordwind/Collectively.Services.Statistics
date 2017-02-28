using System;
using Coolector.Common.Extensions;

namespace Coolector.Services.Statistics.Domain
{
    public class TagStatistics
    {
        public string Name { get; protected set; }

        public RemarksCountStatistics Remarks { get; protected set; }

        protected TagStatistics() { }

        public TagStatistics(string name, RemarksCountStatistics remarks = null)
        {
            if (name.Empty())
            {
                throw new ArgumentException("Category name can not be empty.", nameof(name));
            }
            Name = name;
            Remarks = remarks ?? new RemarksCountStatistics();
        }
    }
}