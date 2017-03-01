using System;

namespace Collectively.Services.Statistics.Domain
{
    public class Vote
    {
        public string UserId { get; protected set; }
        public Guid RemarkId { get; protected set; }
        public bool Positive { get; protected set; }
        public bool Deleted { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected Vote()
        {
            
        }

        protected Vote(Vote vote)
        {
            UserId = vote.UserId;
            RemarkId = vote.RemarkId;
            Positive = vote.Positive;
            CreatedAt = vote.CreatedAt;
        }

        public static Vote CreatePositiveVote(string userId, Guid remarkId)
            => new Vote
            {
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                RemarkId = remarkId,
                Positive = true,
                Deleted = false
            };

        public static Vote CreateNegativeVote(string userId, Guid remarkId)
            => new Vote
            {
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                RemarkId = remarkId,
                Positive = false,
                Deleted = false
            };

        public static Vote CreateDeletedVote(Vote vote)
            => new Vote(vote)
            {
                CreatedAt = DateTime.UtcNow,
                Deleted = true
            };
    }
}