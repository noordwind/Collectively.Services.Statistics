using System;
using Collectively.Common.Extensions;

namespace Collectively.Services.Statistics.Domain
{
    public class CategoryStatistics
    {
        public string Name { get; protected set; }
        public RemarksCountStatistics Remarks { get; protected set; }

        protected CategoryStatistics() { }

        public CategoryStatistics(string name, RemarksCountStatistics remarks = null)
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