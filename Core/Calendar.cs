namespace Core;

public static class Calendar
{
    public static Schedule BusinessSchedule { get; set; } = new Schedule();

    public static bool IsBusinessDay(this DateTime date)
    {
        if (!BusinessSchedule.BusinessDays.Contains(date.DayOfWeek))
        {
            return false;
        }

        if (BusinessSchedule.AnnualHolidays.Any(h => h.Start <= date && date <= h.End))
        {
            return false;
        }

        if (BusinessSchedule.OneTimeHolidays.Any(h => h.Start <= date && date <= h.End))
        {
            return false;
        }

        if(!BusinessSchedule.BusinessHours.Any(h => h.Start <= date.TimeOfDay && date.TimeOfDay <= h.End))
        {
            return false;
        }

        return true;
    }
}