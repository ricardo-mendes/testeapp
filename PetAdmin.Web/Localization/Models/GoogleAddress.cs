using PetAdmin.Web.Localization.Enumerations;
using PetAdmin.Web.Localization.Structs;
using System;
using System.Linq;

namespace PetAdmin.Web.Localization.Models
{
    public class GoogleAddress
    {
        readonly GoogleAddressType type;
        readonly GoogleLocationType locationType;
        readonly GoogleAddressComponent[] components;
        readonly GoogleViewport viewport;
        readonly string placeId;
        string formattedAddress = string.Empty;
        GoogleLocation coordinates;

        public GoogleAddressType Type
        {
            get { return type; }
        }

        public GoogleLocationType LocationType
        {
            get { return locationType; }
        }

        public GoogleAddressComponent[] Components
        {
            get { return components; }
        }

        public GoogleViewport Viewport
        {
            get { return viewport; }
        }

        public string PlaceId
        {
            get { return placeId; }
        }

        public GoogleAddressComponent this[GoogleAddressType type]
        {
            get { return Components.FirstOrDefault(c => c.Types.Contains(type)); }
        }


        public virtual string FormattedAddress
        {
            get { return formattedAddress; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("FormattedAddress is null or blank");

                formattedAddress = value.Trim();
            }
        }

        public virtual GoogleLocation Coordinates
        {
            get { return coordinates; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Coordinates");

                coordinates = value;
            }
        }

        public GoogleAddress(GoogleAddressType type, string formattedAddress, GoogleAddressComponent[] components,
            GoogleLocation coordinates, GoogleViewport viewport, GoogleLocationType locationType, string placeId)
        {
            if (components == null)
                throw new ArgumentNullException("components");

            this.type = type;
            this.components = components;
            this.viewport = viewport;
            this.locationType = locationType;
            this.placeId = placeId;
            this.FormattedAddress = formattedAddress;
            this.Coordinates = coordinates;
        }

        public virtual Distance DistanceBetween(GoogleAddress address)
        {
            return this.Coordinates.DistanceBetween(address.Coordinates);
        }

        public virtual Distance DistanceBetween(GoogleAddress address, DistanceUnits units)
        {
            return this.Coordinates.DistanceBetween(address.Coordinates, units);
        }

        public override string ToString()
        {
            return FormattedAddress;
        }
    }
}
