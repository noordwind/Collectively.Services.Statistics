using Coolector.Common.Domain;

namespace Coolector.Services.Statistics.Domain
{
    public abstract class StatisticsBase : IdentifiableEntity
    {
        public string Name { get; protected set; }
        public int Count { get; protected set; }

        protected StatisticsBase() { }

        protected StatisticsBase(string name, int count = 0)
        {
            Name = name;
            Count = count;
        }

        public virtual void Increase()
        {
            Count++;
        }

        public virtual void Decrease()
        {
            Count--;
        }
    }

    public class CategoryStatistics : StatisticsBase
    {
        protected CategoryStatistics() { }

        public CategoryStatistics(string name, int count = 0)
            :base(name, count) { }
    }

    public class TagStatistics : StatisticsBase
    {
        protected TagStatistics() { }

        public TagStatistics(string name, int count = 0)
            :base(name, count) { }
    }
}