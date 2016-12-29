using System;
using System.Collections.Generic;
using Coolector.Common.Domain;

namespace Coolector.Services.Statistics.Shared.Dto
{
    public class RemarkStatisticsDto
    {
        public Guid RemarkId { get; set; }
        public string Category { get; set; }
        public UserDto Author { get; set; }
        public LocationDto Location { get; set; }
        public IList<string> Tags { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto Resolver { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public bool Resolved { get; set; }  

        public class LocationDto
        {
            public string Address { get; set; }
            public double[] Coordinates { get; set; }
            public string Type { get; set; }
        }

        public class UserDto
        {
            public string Id { get; set;}
            public string Name { get; set;}
        }
    }
}