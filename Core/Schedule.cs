namespace Core;

public class Schedule
{
    private Dictionary<DayOfWeek, List<TimeRange>> BusinessDays = new Dictionary<DayOfWeek, List<TimeRange>>();
    private List<DateRange> AnnualHolidays = new List<DateRange>();
    private List<DateRange> OneTimeHolidays = new List<DateRange>();


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
    
    public List<DateRange> GetOneTimeHolidays()
    {
        return this.OneTimeHolidays;
    }
    
    public void AddOneTimeHoliday(DateRange dateRange)
    {
        this.MergeOneTimeHoliday(dateRange);
    }

    public void SetOneTimeHolidays(List<DateRange> dateRanges)
    {
        this.OneTimeHolidays.Clear();

        foreach (var dateRange in dateRanges)
        {
            this.MergeOneTimeHoliday(dateRange);
        }

        this.OneTimeHolidays = this.OneTimeHolidays.OrderBy(h => h.Start).ToList();
    }

    private void MergeOneTimeHoliday(DateRange dateRangeToAdd)
    {
        MergeHolidays(this.OneTimeHolidays, dateRangeToAdd);
    }

    private void MergeHolidays(List<DateRange> holidays, DateRange dateRangeToAdd)
    {
        var intersectingHolidays = holidays.Where(h => CheckIntersection(h, dateRangeToAdd)).ToList();

        if (intersectingHolidays.Count == 0)
        {
            holidays.Add(dateRangeToAdd);
        }
        else
        {
            var newStartDate = intersectingHolidays.Min(h => h.Start);
            newStartDate = newStartDate < dateRangeToAdd.Start ? newStartDate : dateRangeToAdd.Start;

            var newEndDate = intersectingHolidays.Max(h => h.End);
            newEndDate = newEndDate > dateRangeToAdd.End ? newEndDate : dateRangeToAdd.End;

            var newDateRange = new DateRange()
            {
                Start = newStartDate,
                End = newEndDate
            };

            holidays.Add(newDateRange);
        }

        foreach (var intersectingHoliday in intersectingHolidays)
        {
            holidays.Remove(intersectingHoliday);
        }
    }

    private bool CheckIntersection(DateRange dataRange1, DateRange dataRange2)
    {
        return (dataRange1.Start >= dataRange2.Start && dataRange1.Start <= dataRange2.End) ||
               (dataRange1.End >= dataRange2.Start && dataRange1.End <= dataRange2.End);
    }

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