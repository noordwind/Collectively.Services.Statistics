﻿using System;
using Coolector.Common.Domain;
using Coolector.Common.Extensions;

namespace Coolector.Services.Statistics.Domain
{
    public class User : ValueObject<User>
    {
        public string UserId { get; protected set; }
        public string Name { get; protected set; }

        protected User() { }

        public User(string userId, string name)
        {
            if (userId.Empty())
                throw new ArgumentException("User id can not be empty.", nameof(name));
            if (name.Empty())
                throw new ArgumentException("User name can not be empty.", nameof(name));

            UserId = userId;
            Name = name;
        }

        protected override bool EqualsCore(User other) => UserId.Equals(other.UserId);

        protected override int GetHashCodeCore() => UserId.GetHashCode();
    }
}