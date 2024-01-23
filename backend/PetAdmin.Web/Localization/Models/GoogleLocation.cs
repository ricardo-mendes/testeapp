﻿using PetAdmin.Web.Localization.Enumerations;
using PetAdmin.Web.Localization.Structs;
using System;

namespace PetAdmin.Web.Localization.Models
{
    public class GoogleLocation
    {
        double latitude;
        double longitude;

        public virtual double Latitude
        {
            get { return latitude; }
            set
            {
                if (value < -90 || value > 90)
                    throw new ArgumentOutOfRangeException("Latitude", value, "Value must be between -90 and 90 inclusive.");

                if (double.IsNaN(value))
                    throw new ArgumentException("Latitude must be a valid number.", "Latitude");

                latitude = value;
            }
        }

        public virtual double Longitude
        {
            get { return longitude; }
            set
            {
                if (value < -180 || value > 180)
                    throw new ArgumentOutOfRangeException("Longitude", value, "Value must be between -180 and 180 inclusive.");

                if (double.IsNaN(value))
                    throw new ArgumentException("Longitude must be a valid number.", "Longitude");

                longitude = value;
            }
        }

        protected GoogleLocation()
            : this(0, 0)
        {
        }
        public GoogleLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        protected virtual double ToRadian(double val)
        {
            return (Math.PI / 180.0) * val;
        }

        public virtual Distance DistanceBetween(GoogleLocation location)
        {
            return DistanceBetween(location, DistanceUnits.Miles);
        }

        public virtual Distance DistanceBetween(GoogleLocation location, DistanceUnits units)
        {
            double earthRadius = (units == DistanceUnits.Miles) ? Distance.EarthRadiusInMiles : Distance.EarthRadiusInKilometers;

            double latRadian = ToRadian(location.Latitude - this.Latitude);
            double longRadian = ToRadian(location.Longitude - this.Longitude);

            double a = Math.Pow(Math.Sin(latRadian / 2.0), 2) +
                Math.Cos(ToRadian(this.Latitude)) *
                Math.Cos(ToRadian(location.Latitude)) *
                Math.Pow(Math.Sin(longRadian / 2.0), 2);

            double c = 2.0 * Math.Asin(Math.Min(1, Math.Sqrt(a)));

            double distance = earthRadius * c;
            return new Distance(distance, units);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GoogleLocation);
        }

        public bool Equals(GoogleLocation coor)
        {
            if (coor == null)
                return false;

            return (this.Latitude == coor.Latitude && this.Longitude == coor.Longitude);
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Latitude.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", latitude, longitude);
        }
    }
}
