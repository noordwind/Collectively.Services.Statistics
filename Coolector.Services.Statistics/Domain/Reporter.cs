using Coolector.Common.Domain;

namespace Coolector.Services.Statistics.Domain
{
    public class Reporter : IdentifiableEntity
    {
        public User User { get; protected set; }
        public uint ReportedCount { get; protected set; }

        protected Reporter() { }

        public Reporter(string userId, string username)
        {
            User = new User(userId, username);
            ReportedCount = 1;
        }

        public void IncreaseReportedCount() => ReportedCount++;

        public void DecreaseReportedCount()
        {
            if (ReportedCount == 0)
                return;
            ReportedCount--;
        }
    }
}