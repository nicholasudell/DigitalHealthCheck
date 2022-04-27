using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalHealthCheckCommon
{
    public static class DateTimeExtensions
    {
        public static bool IsBetween(this DateTime value, DateTime lower, DateTime upper) => 
            value >= lower && value <= upper;

        public static bool IsBetween(this DateTime value, TimeSpan lower, TimeSpan upper) => 
            value.TimeOfDay.IsBetween(lower, upper);

        public static bool IsBetween(this TimeSpan value, TimeSpan lower, TimeSpan upper) => 
            value >= lower && value <= upper;
    }
}
