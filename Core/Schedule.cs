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
    
    public bool IsBusinessDay(DateTime date)
    {
        if (!this.BusinessDays.Contains(date.DayOfWeek))
        {
            return false;
        }

        if (this.AnnualHolidays.Any(h => h.Start <= date && date <= h.End))
        {
            return false;
        }

        if (this.OneTimeHolidays.Any(h => h.Start <= date && date <= h.End))
        {
            return false;
        }

        if(!this.BusinessHours.Any(h => h.Start <= date.TimeOfDay && date.TimeOfDay <= h.End))
        {
            return false;
        }

        return true;
    }
}