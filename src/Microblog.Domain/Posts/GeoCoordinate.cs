﻿using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace Microblog.Posts
{
    public class GeoCoordinate : ValueObject
    {
        public double Latitude { get; protected set; }
        public double Longitude { get; protected set; }

        protected GeoCoordinate() { }

        public GeoCoordinate(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}