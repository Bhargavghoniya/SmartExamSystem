using System;

namespace SmartExamSystem.Helpers
{
    public static class TimeHelper
    {
        public static DateTime GetLocalTime()
        {
            try
            {
                // Try Windows timezone ID for IST
                var tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    // Try Linux/macOS timezone ID for IST
                    var tzi = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
                    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
                }
                catch
                {
                    // Ultimate fallback: UTC + 5:30
                    return DateTime.UtcNow.AddHours(5).AddMinutes(30);
                }
            }
        }
    }
}
