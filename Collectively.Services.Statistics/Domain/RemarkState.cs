using System;
using Collectively.Common.Extensions;

namespace Collectively.Services.Statistics.Domain
{
    public class RemarkState
    {
        public string State { get; protected set; }
        public string UserId { get; protected set; }
        public string Description { get; protected set; }
        public Location Location { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected RemarkState()
        {
        }

        public RemarkState(string state, string userId, string description = null, Location location = null)
        {
            if (state.Empty())
            {
                throw new ArgumentException("State can not be empty.", nameof(state));
            }
            if (userId.Empty())
            {
                throw new ArgumentException("User id can not be empty.", nameof(userId));
            }
            if (description?.Length > 2000)
            {
                throw new ArgumentException("Description can not have more than 2000 characters.", 
                                            nameof(description));
            }
            State = state;
            UserId = userId;
            Description = description?.Trim() ?? string.Empty;
            Location = location;
            CreatedAt = DateTime.UtcNow;
        }
    }
}