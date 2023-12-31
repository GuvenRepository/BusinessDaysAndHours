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
        this.MergeAnnualHoliday(dateRange);
    }

    public void SetAnnualHolidays(List<DateRange> dateRanges)
    {
        this.AnnualHolidays.Clear();

        foreach (var dateRange in dateRanges)
        {
            this.MergeAnnualHoliday(dateRange);
        }
    }

    public List<DateRange> GetAnnualHolidays()
    {
        return this.AnnualHolidays;
    }

    private void MergeAnnualHoliday(DateRange dateRangeToAdd)
    {
        if (dateRangeToAdd.End < dateRangeToAdd.Start)
        {
            (dateRangeToAdd.Start, dateRangeToAdd.End) = (dateRangeToAdd.End, dateRangeToAdd.Start);
        }

        if (dateRangeToAdd.Start - dateRangeToAdd.End >= TimeSpan.FromDays(365))
        {
            // Make whole year a holiday
            this.AnnualHolidays.Clear();

            this.AnnualHolidays.Add(new DateRange()
            {
                Start = DateTime.MinValue,
                End = DateTime.MinValue.AddDays(365).Subtract(TimeSpan.FromTicks(1))
            });

            return;
        }

        if (dateRangeToAdd.Start.Year != dateRangeToAdd.End.Year)
        {
            dateRangeToAdd.Start = this.SetYear(dateRangeToAdd.Start, DateTime.MinValue.Year);
            dateRangeToAdd.End = this.SetYear(dateRangeToAdd.End, DateTime.MinValue.Year + 1);

            var christmas = new DateTime(dateRangeToAdd.End.Year, 1, 1);
            
            this.MergeAnnualHoliday(new DateRange()
            {
                Start = dateRangeToAdd.Start,
                End = christmas.Subtract(TimeSpan.FromTicks(1))
            });

            this.MergeAnnualHoliday(new DateRange()
            {
                Start = this.SetYear(christmas, DateTime.MinValue.Year),
                End = this.SetYear(dateRangeToAdd.End, DateTime.MinValue.Year)
            });
        }
        else
        {
            dateRangeToAdd.Start = this.SetYear(dateRangeToAdd.Start, DateTime.MinValue.Year);
            dateRangeToAdd.End = this.SetYear(dateRangeToAdd.End, DateTime.MinValue.Year);
            
            MergeHolidays(ref this.AnnualHolidays, dateRangeToAdd);
        }

    }

    private DateTime SetYear(DateTime date, int year)
    {
        return new DateTime(year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
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
    }

    private void MergeOneTimeHoliday(DateRange dateRangeToAdd)
    {
        MergeHolidays(ref this.OneTimeHolidays, dateRangeToAdd);
    }

    private void MergeHolidays(ref List<DateRange> holidays, DateRange dateRangeToAdd)
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

        holidays = holidays.OrderBy(h => h.Start).ToList();
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