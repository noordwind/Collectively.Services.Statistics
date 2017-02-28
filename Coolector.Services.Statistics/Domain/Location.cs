using System;

namespace Coolector.Services.Statistics.Domain
{
    public class Location
    {
        public double Longitude => Coordinates[0];
        public double Latitude => Coordinates[1];
        public double[] Coordinates { get; protected set; }
        public string Type { get; protected set; }
        public string Address { get; protected set; }

        protected Location()
        {
        }

        public Location(double latitude, double longitude, string address = null)
        {
            if (latitude > 90 || latitude < -90)
            {
                throw new ArgumentException($"Invalid latitude {latitude}", nameof(latitude));
            }
            if (longitude > 180 || longitude < -180)
            {
                throw new ArgumentException($"Invalid longitude {longitude}", nameof(longitude));
            }
            Type = "Point";
            Coordinates = new[] { longitude, latitude };
            Address = address;
        }
    }
}