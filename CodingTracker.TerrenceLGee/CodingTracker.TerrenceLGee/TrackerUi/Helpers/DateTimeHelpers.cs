namespace CodingTracker.TerrenceLGee.TrackerUi.Helpers;

public static class DateTimeHelpers
{
    public static bool IsValidDate(DateTime date)
    {
        var year = date.Year;
        var month = date.Month;
        var dayOfMonth = date.Day;
        var daysInEachMonth = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        if (IsLeapYear(year))
        {
            daysInEachMonth[1]++;
        }

        if (year < DateTime.Now.Year)
        {
            return false;
        }

        if (month < 1 || month > 12 || month < DateTime.Now.Month)
        {
            return false;
        }

        if (dayOfMonth < 1 || dayOfMonth > daysInEachMonth[month - 1] || dayOfMonth < DateTime.Now.Day)
        {
            return false;
        }

        var minute = date.Minute;
        var hour = date.Hour;

        if (minute < 0 || minute > 59)
        {
            return false;
        }

        if (hour < 0 || hour > 23)
        {
            return false;
        }
        
        return true;
    }
    
    public static bool IsValidEndDate(DateTime startDate, DateTime endDate) => startDate <= endDate;
    private static bool IsLeapYear(int year) => year % 400 == 0 || (year % 4 == 0 && year % 100 != 0);
}