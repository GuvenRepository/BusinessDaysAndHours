namespace Core;

public class Schedule
{
    private Dictionary<DayOfWeek, List<TimeRange>> BusinessDays = new Dictionary<DayOfWeek, List<TimeRange>>();
    private List<DateRange> AnnualHolidays = new List<DateRange>();
    private List<DateRange> OneTimeHolidays = new List<DateRange>();


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
    
    public void AddBusinessDay(DayOfWeek dayOfWeek, List<TimeRange> timeRanges)
    {
        this.BusinessDays.Add(dayOfWeek, timeRanges);
    }
    
    public void SetBusinessDays(Dictionary<DayOfWeek, List<TimeRange>> businessDays)
    {
        this.BusinessDays = businessDays;
    }

    public void AddAnnualHoliday(DateRange dateRange)
    {
        this.AnnualHolidays.Add(dateRange);
    }
    
    public void SetAnnualHolidays(List<DateRange> dateRanges)
    {
        this.AnnualHolidays.AddRange(dateRanges);
    }

    public void AddOneTimeHoliday(DateRange dateRange)
    {
        this.OneTimeHolidays.Add(dateRange);
    }
    
    public void SetOneTimeHolidays(List<DateRange> dateRanges)
    {
        this.OneTimeHolidays.AddRange(dateRanges);
    }
}