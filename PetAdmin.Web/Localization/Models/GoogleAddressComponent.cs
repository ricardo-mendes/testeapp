using PetAdmin.Web.Localization.Enumerations;
using System;
using System.Linq;

namespace PetAdmin.Web.Localization.Models
{
    public class GoogleAddressComponent
    {
        public GoogleAddressType[] Types { get; private set; }
        public string LongName { get; private set; }
        public string ShortName { get; private set; }

        public int Order
        {
            get
            {
                return SortMainTypes();
            }
        }

        public GoogleAddressComponent(GoogleAddressType[] types, string longName, string shortName)
        {
            if (types == null)
                throw new ArgumentNullException("types");

            if (types.Length < 1)
                throw new ArgumentException("Value cannot be empty.", "types");

            this.Types = types;
            this.LongName = longName;
            this.ShortName = shortName;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Types[0], LongName);
        }

        private int SortMainTypes()
        {
            int order = 0;
            if (this != null && this.Types != null)
            {
                var mainType = this.Types.FirstOrDefault();
                switch (mainType)
                {
                    case GoogleAddressType.Country:
                        order = 6;
                        break;
                    case GoogleAddressType.AdministrativeAreaLevel1:
                        order = 5;
                        break;
                    case GoogleAddressType.AdministrativeAreaLevel2:
                        order = 4;
                        break;
                    case GoogleAddressType.AdministrativeAreaLevel3:
                        order = 3;
                        break;
                    case GoogleAddressType.Locality:
                        order = 2;
                        break;
                    case GoogleAddressType.SubLocality:
                        order = 1;
                        break;
                    default:
                        break;
                }

            }

            return order;
        }

        public void setAdress(string shortName, string longName)
        {
            this.ShortName = shortName;
            this.LongName = longName;
        }
    }
}
