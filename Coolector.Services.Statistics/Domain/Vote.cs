using System;

namespace Coolector.Services.Statistics.Domain
{
    public class Vote
    {
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public Guid RemarkId { get; set; }
        public bool Positive { get; set; }
        public bool Deleted { get; set; }

        protected Vote()
        {
            
        }

        protected Vote(Vote vote)
        {
            UserId = vote.UserId;
            RemarkId = vote.RemarkId;
            Positive = vote.Positive;
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