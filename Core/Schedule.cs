namespace Core;

public class Schedule
{
    public Dictionary<DayOfWeek, List<TimeRange>> BusinessDays { get; set; } = new Dictionary<DayOfWeek, List<TimeRange>>();
    public List<DateRange> AnnualHolidays { get; set; } = new List<DateRange>();
    public List<DateRange> OneTimeHolidays { get; set; } = new List<DateRange>();


    public bool IsBusinessDay(DateTime date)
    {
        if (this.AnnualHolidays.Any(h => h.Start <= date && date <= h.End))
        {
            return false;
        }

        if (this.OneTimeHolidays.Any(h => h.Start <= date && date <= h.End))
        {
            return false;
        }
        
        if (!this.BusinessDays.Keys.Contains(date.DayOfWeek))
        {
            return false;
        }
        
        if (!this.BusinessDays[date.DayOfWeek].Any(h => h.Start <= date.TimeOfDay && date.TimeOfDay <= h.End))
        {
            return false;
        }

        return true;
    }
}