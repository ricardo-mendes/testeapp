using NodaTime;
using System;

namespace PetAdmin.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToBrazilTimeZoneDateTime(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return dateTime;

            var timeZone = DateTimeZoneProviders.Tzdb["America/Sao_Paulo"];

            var instant = Instant.FromDateTimeUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
            return instant.InZone(timeZone).ToDateTimeUnspecified();
        }
    }
}
