using Core;
using NUnit.Framework;

namespace UnitTests;

public class Tests
{
    [TestCase("2024-01-01T09:00:00", ExpectedResult = false)]
    [TestCase("2024-01-02T09:00:00", ExpectedResult = true)]
    [TestCase("2024-01-02T12:00:00", ExpectedResult = true)]
    [TestCase("2024-01-02T12:30:00", ExpectedResult = false)]
    [TestCase("2024-01-05T14:00:00", ExpectedResult = false)]
    [TestCase("2024-01-06T14:00:00", ExpectedResult = false)]
    public bool IsBusinessDay(string dateStr)
    {
        var date = DateTime.Parse(dateStr);

        var schedule = new Schedule();

        var businessDays = new Dictionary<DayOfWeek, List<TimeRange>>()
        {
            {
                DayOfWeek.Monday, new List<TimeRange>()
                {
                    new TimeRange()
                    {
                        Start = new TimeSpan(9, 0, 0),
                        End = new TimeSpan(12, 0, 0)
                    },
                    new TimeRange()
                    {
                        Start = new TimeSpan(13, 0, 0),
                        End = new TimeSpan(18, 0, 0)
                    }
                }
            },
            {
                DayOfWeek.Tuesday, new List<TimeRange>()
                {
                    new TimeRange()
                    {
                        Start = new TimeSpan(9, 0, 0),
                        End = new TimeSpan(12, 0, 0)
                    },
                    new TimeRange()
                    {
                        Start = new TimeSpan(13, 0, 0),
                        End = new TimeSpan(18, 0, 0)
                    }
                }
            },
            {
                DayOfWeek.Wednesday, new List<TimeRange>()
                {
                    new TimeRange()
                    {
                        Start = new TimeSpan(9, 0, 0),
                        End = new TimeSpan(12, 0, 0)
                    },
                    new TimeRange()
                    {
                        Start = new TimeSpan(13, 0, 0),
                        End = new TimeSpan(18, 0, 0)
                    }
                }
            },
            {
                DayOfWeek.Thursday, new List<TimeRange>()
                {
                    new TimeRange()
                    {
                        Start = new TimeSpan(9, 0, 0),
                        End = new TimeSpan(12, 0, 0)
                    },
                    new TimeRange()
                    {
                        Start = new TimeSpan(13, 0, 0),
                        End = new TimeSpan(18, 0, 0)
                    }
                }
            },
            {
                DayOfWeek.Friday, new List<TimeRange>()
                {
                    new TimeRange()
                    {
                        Start = new TimeSpan(9, 0, 0),
                        End = new TimeSpan(12, 0, 0)
                    },
                    new TimeRange()
                    {
                        Start = new TimeSpan(13, 0, 0),
                        End = new TimeSpan(18, 0, 0)
                    }
                }
            }
        };

        schedule.SetBusinessDays(businessDays);

        var christmas = new DateRange()
        {
            Start = new DateTime(2024, 1, 1, 0, 0, 0),
            End = new DateTime(2024, 1, 2, 0, 0, 0)
        };

        schedule.AddAnnualHoliday(christmas);

        var happyFriday = new DateRange()
        {
            Start = new DateTime(2024, 1, 5, 13, 0, 0),
            End = new DateTime(2024, 1, 5, 18, 0, 0)
        };

        schedule.AddOneTimeHoliday(happyFriday);

        return schedule.IsBusinessDay(date);
    }

    [Test]
    public void MergeOneTimeHolidays()
    {
        var schedule = new Schedule();

        var holidays = new List<DateRange>()
        {
            new DateRange()
            {
                Start = new DateTime(2024, 1, 5, 13, 0, 0),
                End = new DateTime(2024, 1, 5, 18, 0, 0)
            },
            new DateRange()
            {
                Start = new DateTime(2024, 1, 6, 13, 0, 0),
                End = new DateTime(2024, 1, 6, 18, 0, 0)
            }
        };

        schedule.SetOneTimeHolidays(holidays);

        Assert.IsTrue(schedule.GetOneTimeHolidays().Count == 2);


        holidays = new List<DateRange>()
        {
            new DateRange()
            {
                Start = new DateTime(2024, 1, 5, 13, 0, 0),
                End = new DateTime(2024, 1, 5, 18, 0, 0)
            },
            new DateRange()
            {
                Start = new DateTime(2024, 1, 5, 18, 0, 0),
                End = new DateTime(2024, 1, 6, 18, 0, 0)
            }
        };

        schedule.SetOneTimeHolidays(holidays);

        Assert.IsTrue(schedule.GetOneTimeHolidays().Count == 1);
    }

    [Test]
    public void MergeAnnualHolidays()
    {
        var schedule = new Schedule();

        var holidays = new List<DateRange>()
        {
            new DateRange()
            {
                Start = new DateTime(2023, 10, 5, 13, 0, 0),
                End = new DateTime(2024, 1, 5, 18, 0, 0)
            }
        };

        schedule.SetAnnualHolidays(holidays);

        Assert.IsTrue(schedule.GetAnnualHolidays().Count == 2);


        holidays = new List<DateRange>()
        {
            new DateRange()
            {
                Start = new DateTime(2023, 10, 5, 13, 0, 0),
                End = new DateTime(2024, 1, 5, 18, 0, 0)
            },
            new DateRange()
            {
                Start = new DateTime(2024, 1, 5, 18, 0, 0),
                End = new DateTime(2024, 1, 6, 18, 0, 0)
            },
        };

        schedule.SetAnnualHolidays(holidays);

        Assert.IsTrue(schedule.GetAnnualHolidays().Count == 2);
    }
}