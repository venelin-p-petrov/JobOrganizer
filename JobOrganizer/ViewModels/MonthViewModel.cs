using JobOrganizer.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JobOrganizer.ViewModels
{
    public class MonthViewModel : BindableBase
    {
        private DateTime firstDay;
        private string pageTitle;
        private ObservableCollection<WeekViewModel> weeks;

        public string PageTitle
        {
            get
            {
                return this.pageTitle;
            }
            set
            {
                this.pageTitle = value;
                this.OnPropertyChanged();
            }
        }

        public IList<WeekViewModel> Weeks
        {
            get
            {
                return this.weeks;
            }
        }

        public DateTime FirstDay
        {
            get
            {
                return this.firstDay;
            }
        }

        public DayViewModel Today
        {
            get
            {
                foreach (WeekViewModel week in this.weeks)
                {
                    foreach (DayViewModel day in week.Days)
                    {
                        if (day.Date == DateTime.Today)
                        {
                            return day;
                        }
                    }
                }
                return null;
            }
        }

        public MonthViewModel()
            : this(DateTime.Today)
        {
        }

        public MonthViewModel(DateTime firstDay)
        {
            this.firstDay = new DateTime(firstDay.Year, firstDay.Month, 1);
            this.pageTitle = this.firstDay.ToString("MMMM yyyy");
            this.weeks = new ObservableCollection<WeekViewModel>();
            this.CreateWeeks();
        }

        private void CreateWeeks()
        {
            var lastDayOfMonth = new DateTime(this.firstDay.Year, this.firstDay.Month,
                                                DateTime.DaysInMonth(this.firstDay.Year, this.firstDay.Month));

            var startDay = this.firstDay;
            var endDay = lastDayOfMonth;

            if (this.firstDay.DayOfWeek != DayOfWeek.Monday)
            {
                int dayDiff = (int)this.firstDay.DayOfWeek - 1;

                if (dayDiff < 0)
                {
                    dayDiff += 7;
                }

                startDay = this.firstDay.Subtract(new TimeSpan(dayDiff, 0, 0, 0));
            }

            if (lastDayOfMonth.DayOfWeek != DayOfWeek.Sunday)
            {
                int dayDiff = 7 - (int)lastDayOfMonth.DayOfWeek;

                if (dayDiff >= 7)
                {
                    dayDiff -= 7;
                }

                endDay = endDay.AddDays(dayDiff);
            }

            var current = startDay;
            do
            {
                var week = new WeekViewModel(current);

                this.weeks.Add(week);
                current = current.AddDays(7);
            } while (current <= endDay);
        }
    }
}
