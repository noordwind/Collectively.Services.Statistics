using System;
using Collectively.Common.Extensions;

namespace Collectively.Services.Statistics.Domain
{
    public class User 
    {
        public string UserId { get; protected set;}
        public string Name { get; protected set;}

        protected User()
        {
        }

        public User(string userId, string name)
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
    }
}