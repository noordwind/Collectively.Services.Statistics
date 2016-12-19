using Coolector.Common.Domain;

namespace Coolector.Services.Statistics.Domain
{
    public class Resolver : IdentifiableEntity
    {
        public User User { get; protected set; }
        public uint ResolvedCount { get; protected set; }

        protected Resolver() { }

        public Resolver(string userId, string username)
        {
            User = new User(userId, username);
            ResolvedCount = 1;
        }

        public void IncreaseResolvedCount() => ResolvedCount++;

        public void DecreaseResolvedCount()
        {
            if (ResolvedCount == 0)
                return;
            ResolvedCount--;
        }
    }
}