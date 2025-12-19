namespace CloneIntime.Utils.helpers
{
    public class AdminHelper
    {

        private static readonly Dictionary<DayOfWeek, WeekEnum> _weekDaysComparer = new()
        {
            { DayOfWeek.Monday, WeekEnum.Monday },
            { DayOfWeek.Tuesday, WeekEnum.Tuesday },
            { DayOfWeek.Wednesday, WeekEnum.Wednesday },
            { DayOfWeek.Thursday, WeekEnum.Thursday },
            { DayOfWeek.Friday, WeekEnum.Friday },
            { DayOfWeek.Saturday, WeekEnum.Saturday },
            { DayOfWeek.Sunday, WeekEnum.Sunday }
        };

        public WeekEnum GetWeekDayNameFromDate(DateTime day)
        {
            return _weekDaysComparer.GetValueOrDefault(day.DayOfWeek, WeekEnum.Sunday);
        }
    }
}
