using System;
using Coolector.Common.Domain;
using Coolector.Common.Extensions;

namespace Coolector.Services.Statistics.Domain
{
    public class UserStatistics : IdentifiableEntity
    {
        public string UserId { get; protected set; }
        public string Name { get; protected set; }
        public uint ReportedCount { get; protected set; }
        public uint ResolvedCount { get; protected set; }
        public uint DeletedCount { get; protected set; }

        protected UserStatistics() { }

        public UserStatistics(string userId, string name)
        {
            if (userId.Empty())
                throw new ArgumentException("User id can not be empty.", nameof(name));
            if (name.Empty())
                throw new ArgumentException("User name can not be empty.", nameof(name));

            UserId = userId;
            Name = name;
            ReportedCount = 0;
            ResolvedCount = 0;
            DeletedCount = 0;
        }

        public void IncreaseReportedCount() => ReportedCount++;

        public void DecreaseReportedCount()
        {
            if (ReportedCount == 0)
                return;

            ReportedCount--;
        }

        public void IncreaseResolvedCount() => ResolvedCount++;

        public void DecreaseResolvedCount()
        {
            if (ResolvedCount == 0)
                return;

            ResolvedCount--;
        }

        public void IncreaseDeletedCount() => DeletedCount++;
    }
}