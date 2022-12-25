using Microsoft.EntityFrameworkCore;
using System;

namespace AutoVerleih.Filter
{
    [Keyless]
    public static class DefaultFilter
    {
        public static DateTime DT_From { get; set; }
        public static DateTime DT_To { get; set; }
        public static bool IncludeInactive { get; set; }
        public static int AnzLine { get; set; }
        public static bool IsOnlyShowRentCars { get; set; }
    }
}
