using System;
using System.Collections.Generic;
using Coolector.Common.Domain;
using Coolector.Common.Extensions;

namespace Coolector.Services.Statistics.Domain
{
    public class RemarkStatistics : IdentifiableEntity
    {
        private ISet<string> _tags = new HashSet<string>();
        public Guid RemarkId { get; protected set; }
        public string AuthorId { get; protected set; }
        public string Category { get; protected set; }
        public RemarkLocation Location { get; protected set; }
        public string Description { get; protected set; }
        public string ResolverId { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? ResolvedAt { get; protected set; }

        public IEnumerable<string> Tags
        {
            get { return _tags; }
            protected set { _tags = new HashSet<string>(value); }
        }

        protected RemarkStatistics()
        {
        }

        public RemarkStatistics(Guid remarkId)
        {
            RemarkId = remarkId;
        }

        public class RemarkLocation
        {
            public double Longitude => Coordinates[0];
            public double Latitude => Coordinates[1];
            public double[] Coordinates { get; protected set; }
            public string Type { get; protected set; }
            public string Address { get; protected set; }

            protected RemarkLocation()
            {
            }

            public RemarkLocation(double latitude, double longitude, string address = null)
            {
                if (latitude > 90 || latitude < -90)
                    throw new ArgumentException($"Invalid latitude {latitude}", nameof(latitude));
                if (longitude > 180 || longitude < -180)
                    throw new ArgumentException($"Invalid longitude {longitude}", nameof(longitude));

                Type = "Point";
                Coordinates = new[] { longitude, latitude };
                Address = address;
            }
        }
    }
}