using Coolector.Common.Domain;

namespace Coolector.Services.Statistics.Domain
{
    public abstract class StatisticsBase : IdentifiableEntity
    {
        public string Name { get; protected set; }
        public uint CreatedCount { get; protected set; }
        public uint ResolvedCount { get; protected set; }
        public uint DeletedCount { get; protected set; }
        public uint ReportedCount { get; protected set; }

        protected StatisticsBase() { }

        protected StatisticsBase(string name, uint created = 0, uint resolved = 0, uint deleted = 0)
        {
            Name = name;
            CreatedCount = created;
            ResolvedCount = resolved;
            DeletedCount = deleted;
            ReportedCount = created - deleted;
        }

        public virtual void IncreaseCreated()
        {
            CreatedCount++;
            ReportedCount = CreatedCount - DeletedCount;
        }
        public virtual void IncreaseResolved()
        {
            ResolvedCount++;
        }
        public virtual void IncreaseDeleted()
        {
            DeletedCount++;
            ReportedCount = CreatedCount - DeletedCount;
        }
    }
}