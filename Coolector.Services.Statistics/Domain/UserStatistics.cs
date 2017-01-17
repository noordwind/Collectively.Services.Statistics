using System;
using System.Collections.Generic;
using Coolector.Common.Domain;
using Coolector.Common.Extensions;

namespace Coolector.Services.Statistics.Domain
{
    public class UserStatistics : StatisticsBase
    {
        public string UserId { get; protected set; }
        public IList<VoteStatistics> Votes { get; protected set; }

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
            Votes = new List<VoteStatistics>();
        }

        public void AddVote(VoteStatistics vote)
        {
            if (Votes == null)
                Votes = new List<VoteStatistics>();

            Votes.Add(vote);
        }
    }
}