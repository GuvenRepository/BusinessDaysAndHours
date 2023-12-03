namespace Core;

public class Schedule
{
    public List<DayOfWeek> BusinessDays { get; set; } = new List<DayOfWeek>
    {
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday
    };

    public List<TimeRange> BusinessHours { get; set; } = new List<TimeRange>()
    {
        new TimeRange()
        {
            Start = new TimeSpan(9, 0, 0),
            End = new TimeSpan(12, 0,0)
        },
        new TimeRange()
        {
            Start = new TimeSpan(13, 0, 0),
            End = new TimeSpan(18, 0, 0)
        }
    };

    public List<DateRange> AnnualHolidays { get; set; } = new List<DateRange>();

    public List<DateRange> OneTimeHolidays { get; set; } = new List<DateRange>();
}