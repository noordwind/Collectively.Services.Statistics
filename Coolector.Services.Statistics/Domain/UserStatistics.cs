using System;
using System.Collections.Generic;
using Coolector.Common.Extensions;

namespace Coolector.Services.Statistics.Domain
{
    public class UserStatistics : RemarksCountStatisticsBase
    {
        private ISet<Vote> _votes = new HashSet<Vote>();
        public string UserId { get; protected set; }
        public string Name { get; protected set; }

        public IEnumerable<Vote> Votes
        {
            get { return _votes; }
            protected set { _votes = new HashSet<Vote>(value); }
        }

        protected UserStatistics() { }

        public UserStatistics(string userId, string name)
        {
            if (userId.Empty())
            {
                throw new ArgumentException("User id can not be empty.", nameof(name));
            }
            if (name.Empty())
            {
                throw new ArgumentException("User name can not be empty.", nameof(name));
            }
            UserId = userId;
            Name = name;
        }

        public void AddVote(Vote vote)
        {
            _votes.Add(vote);
        }
    }
}