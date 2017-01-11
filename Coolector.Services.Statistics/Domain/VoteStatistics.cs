using System;

namespace Coolector.Services.Statistics.Domain
{
    public class VoteStatistics
    {
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public Guid RemarkId { get; set; }
        public bool Positive { get; set; }
        public bool Deleted { get; set; }

        protected VoteStatistics()
        {
            
        }

        protected VoteStatistics(VoteStatistics vote)
        {
            UserId = vote.UserId;
            RemarkId = vote.RemarkId;
            Positive = vote.Positive;
        }

        public static VoteStatistics CreatePositiveVote(string userId, Guid remarkId)
            => new VoteStatistics
            {
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                RemarkId = remarkId,
                Positive = true,
                Deleted = false
            };

        public static VoteStatistics CreateNegativeVote(string userId, Guid remarkId)
            => new VoteStatistics
            {
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                RemarkId = remarkId,
                Positive = false,
                Deleted = false
            };

        public static VoteStatistics CreateDeletedVote(VoteStatistics vote)
            => new VoteStatistics(vote)
            {
                CreatedAt = DateTime.UtcNow,
                Deleted = true
            };
    }
}