

using FluentValidator;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PetAdmin.Web.Localization.Enumerations;
using PetAdmin.Web.Localization.Models;
using PetAdmin.Web.Models.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PetAdmin.Web.Localization
{
    public class LocationGoogleService : Notifiable
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LocationGoogleService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task SetLatitudeAndLongitudeAsync(Location location)
        {
            if (location != null && !string.IsNullOrWhiteSpace(location.CityName) 
                && !string.IsNullOrWhiteSpace(location.Neighborhood) && !string.IsNullOrWhiteSpace(location.StreetName))
            {
                var address = GetAddressText(location);

                var httpClient = _httpClientFactory.CreateClient();
                //var requestUri = $"{ServiceUrl}&address={address}&language={language}";
                var requestUri = $"https://maps.googleapis.com/maps/api/geocode/json?sensor=false&key=AIzaSyDuFq1VPK98imq6IYu06noc4BzyxzqapRo&address={address}&language=pt-br";
                var responseMessage = await httpClient.GetAsync(requestUri);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var resultData = await responseMessage.Content.ReadAsStringAsync();

                    var googleAddress = GetMapsGeocodeResult(resultData);

                    if (googleAddress != null && googleAddress.Coordinates != null)
                    {
                        location.Latitude = googleAddress.Coordinates.Latitude;
                        location.Longitue = googleAddress.Coordinates.Longitude;
                    }
                    //else
                        //AddNotification("Location", "Endereço Inválido");
                }
                //else
                    //AddNotification("Location", "Endereço Inválido");
            }
        }

        private string GetAddressText(Location location)
        {
            var address = new StringBuilder();

            address.Append($"{location.StreetName}, {location.StreetNumber}");

            if (!string.IsNullOrEmpty(location.Neighborhood))
                address.Append($" - {location.Neighborhood}");

            if (!string.IsNullOrEmpty(location.CityName))
                address.Append($", {location.CityName}");

            if (!string.IsNullOrEmpty(location.StateCode))
                address.Append($" - {location.StateCode}");

            if (!string.IsNullOrEmpty(location.PostalCode))
                address.Append($", {location.PostalCode}");

            address.Append($", Brasil");

            return address.ToString();
        }

        private GoogleAddress GetMapsGeocodeResult(string resultData)
        {
            var result = JsonConvert.DeserializeObject<MapsGeocodeResult>(resultData);

            var status = EvaluateStatus(result.status);

            if (status != GoogleStatus.Ok && status != GoogleStatus.ZeroResults)
                return null; //error

            if (status == GoogleStatus.Ok)
                return ParseAddresses(result.results).FirstOrDefault();

            return null;
        }

        /// <remarks>
        /// http://code.google.com/apis/maps/documentation/geocoding/#StatusCodes
        /// </remarks>
        private GoogleStatus EvaluateStatus(string status)
        {
            switch (status)
            {
                case "OK": return GoogleStatus.Ok;
                case "ZERO_RESULTS": return GoogleStatus.ZeroResults;
                case "OVER_QUERY_LIMIT": return GoogleStatus.OverQueryLimit;
                case "REQUEST_DENIED": return GoogleStatus.RequestDenied;
                case "INVALID_REQUEST": return GoogleStatus.InvalidRequest;
                default: return GoogleStatus.Error;
            }
        }

        private List<GoogleAddress> ParseAddresses(List<Result> geocodes)
        {
            var result = new List<GoogleAddress>();

            foreach (var geocode in geocodes)
            {

                GoogleAddressType type = EvaluateType(geocode.types.FirstOrDefault());
                string placeId = geocode.place_id;
                string formattedAddress = geocode.formatted_address;

                var components = ParseComponents(geocode.address_components).ToArray();

                var geoLocation = geocode.geometry.location;
                var geoViewport = geocode.geometry.viewport;
                var geoLocationType = geocode.geometry.location_type;

                GoogleLocation coordinates = new GoogleLocation(geoLocation.lat, geoLocation.lng);
                GoogleLocation neCoordinates = new GoogleLocation(geoViewport.northeast.lat, geoViewport.northeast.lng);
                GoogleLocation swCoordinates = new GoogleLocation(geoViewport.southwest.lat, geoViewport.southwest.lng);

                var viewport = new GoogleViewport { Northeast = neCoordinates, Southwest = swCoordinates };

                GoogleLocationType locationType = EvaluateLocationType(geoLocationType);

                result.Add(new GoogleAddress(type, formattedAddress, components, coordinates, viewport, locationType, placeId));
            }

            return result;
        }

        /// <remarks>
        /// http://code.google.com/apis/maps/documentation/geocoding/#Types
        /// </remarks>
        private GoogleAddressType EvaluateType(string type)
        {
            switch (type)
            {
                case "street_address": return GoogleAddressType.StreetAddress;
                case "route": return GoogleAddressType.Route;
                case "intersection": return GoogleAddressType.Intersection;
                case "political": return GoogleAddressType.Political;
                case "country": return GoogleAddressType.Country;
                case "administrative_area_level_1": return GoogleAddressType.AdministrativeAreaLevel1;
                case "administrative_area_level_2": return GoogleAddressType.AdministrativeAreaLevel2;
                case "administrative_area_level_3": return GoogleAddressType.AdministrativeAreaLevel3;
                case "colloquial_area": return GoogleAddressType.ColloquialArea;
                case "locality": return GoogleAddressType.Locality;
                case "sublocality": return GoogleAddressType.SubLocality;
                case "neighborhood": return GoogleAddressType.Neighborhood;
                case "premise": return GoogleAddressType.Premise;
                case "subpremise": return GoogleAddressType.Subpremise;
                case "postal_code": return GoogleAddressType.PostalCode;
                case "natural_feature": return GoogleAddressType.NaturalFeature;
                case "airport": return GoogleAddressType.Airport;
                case "park": return GoogleAddressType.Park;
                case "point_of_interest": return GoogleAddressType.PointOfInterest;
                case "post_box": return GoogleAddressType.PostBox;
                case "street_number": return GoogleAddressType.StreetNumber;
                case "floor": return GoogleAddressType.Floor;
                case "room": return GoogleAddressType.Room;
                case "postal_town": return GoogleAddressType.PostalTown;
                case "establishment": return GoogleAddressType.Establishment;
                case "sublocality_level_1": return GoogleAddressType.SubLocalityLevel1;
                case "sublocality_level_2": return GoogleAddressType.SubLocalityLevel2;
                case "sublocality_level_3": return GoogleAddressType.SubLocalityLevel3;
                case "sublocality_level_4": return GoogleAddressType.SubLocalityLevel4;
                case "sublocality_level_5": return GoogleAddressType.SubLocalityLevel5;
                case "postal_code_suffix": return GoogleAddressType.PostalCodeSuffix;

                default: return GoogleAddressType.Unknown;
            }
        }

        private IEnumerable<GoogleAddressComponent> ParseComponents(List<AddressComponent> addressesComponents)
        {
            foreach (var address in addressesComponents)
            {
                string longName = address.long_name;
                string shortName = address.short_name;
                var types = ParseComponentTypes(address.types).ToArray();

                if (types.Any()) //don't return an address component with no type
                    yield return new GoogleAddressComponent(types, longName, shortName);
            }
        }

        private IEnumerable<GoogleAddressType> ParseComponentTypes(List<string> componentTrpes)
        {
            foreach (var item in componentTrpes)
                yield return EvaluateType(item);
        }

        /// <remarks>
        /// https://developers.google.com/maps/documentation/geocoding/?csw=1#Results
        /// </remarks>
        private GoogleLocationType EvaluateLocationType(string type)
        {
            switch (type)
            {
                case "ROOFTOP": return GoogleLocationType.Rooftop;
                case "RANGE_INTERPOLATED": return GoogleLocationType.RangeInterpolated;
                case "GEOMETRIC_CENTER": return GoogleLocationType.GeometricCenter;
                case "APPROXIMATE": return GoogleLocationType.Approximate;

                default: return GoogleLocationType.Unknown;
            }
        }
    }
}
