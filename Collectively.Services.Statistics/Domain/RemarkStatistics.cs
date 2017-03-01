using System;
using System.Collections.Generic;
using Collectively.Common.Domain;

namespace Collectively.Services.Statistics.Domain
{
    public class RemarkStatistics : IdentifiableEntity
    {
        private ISet<RemarkState> _states = new HashSet<RemarkState>();
        private ISet<string> _tags = new HashSet<string>();
        private ISet<Vote> _votes = new HashSet<Vote>();
        public Guid RemarkId { get; protected set; }
        public User Author { get; protected set; }
        public string Category { get; protected set; }
        public Location Location { get; protected set; }
        public string Description { get; protected set; }
        public RemarkState State { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public IEnumerable<RemarkState> States
        {
            get { return _states; }
            protected set { _states = new HashSet<RemarkState>(value); }
        }

        public IEnumerable<string> Tags
        {
            get { return _tags; }
            protected set { _tags = new HashSet<string>(value); }
        }

        public IEnumerable<Vote> Votes
        {
            get { return _votes; }
            protected set { _votes = new HashSet<Vote>(value); }
        }

        protected RemarkStatistics()
        {
        }

        public RemarkStatistics(Guid remarkId, string category,
            string authorId, string authorName, DateTime createdAt, string state,
            double latitude, double longitude, string address = null,
            string description = null, IEnumerable<string> tags = null)
        {
            RemarkId = remarkId;
            Category = category;
            Author = new User(authorId, authorName);
            CreatedAt = createdAt;
            Description = description;
            Tags = tags ?? new HashSet<string>();
            Location = new Location(latitude, longitude, address);
            var remarkState = new RemarkState(state, authorId, location: Location);
            AddState(remarkState);
        }

        public void AddVote(Vote vote)
        {
            _votes.Add(vote);
        }

        public void AddState(RemarkState state)
        {
            State = state;
            _states.Add(state);
        }
    }
}