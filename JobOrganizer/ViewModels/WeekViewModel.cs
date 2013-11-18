using JobOrganizer.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JobOrganizer.Data;

namespace JobOrganizer.ViewModels
{
    public class WeekViewModel : BindableBase
    {
        private DateTime firstDay;
        private string pageTitle;
        private ObservableCollection<DayViewModel> days;

        public DateTime FirstDay
        {
            get
            {
                return this.firstDay;
            }
        }

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

        public IList<DayViewModel> Days
        {
            get
            {
                return this.days;
            }
        }

        public DayViewModel Today
        {
            get
            {
                foreach (DayViewModel day in this.days)
                {
                    if (day.Date == DateTime.Today)
                    {
                        return day;
                    }
                }
                return null;
            }
        }

        public WeekViewModel()
            : this(DateTime.Today)
        {
        }

        public WeekViewModel(DateTime firstDay)
        {
            this.firstDay = firstDay;
            if (this.firstDay.DayOfWeek != DayOfWeek.Monday)
            {
                int dayDiff = (int)this.firstDay.DayOfWeek - 1;

                if (dayDiff < 0)
                {
                    dayDiff += 7;
                }

                this.firstDay = this.firstDay.AddDays(-dayDiff);
            }
            this.pageTitle = "Current Week";
            this.days = new ObservableCollection<DayViewModel>();
            this.CreateDays();
        }

        private void CreateDays()
        {
            for (int i = 0; i < 7; i++)
            {
                var date = this.firstDay.AddDays(i);

                var tasksForDay = new List<TaskViewModel>();

                if (TaskRepository.Tasks != null)
                {
                    tasksForDay = TaskRepository.Tasks.AsQueryable()
                    .Where(t => t.Start >= date && t.Start < date.AddDays(1))
                    .OrderBy(t => t.Start)
                    .Select(t => new TaskViewModel()
                    {
                        Title = t.Title,
                        Message = t.Message,
                        Start = t.Start,
                        End = t.End,
                        Until = t.Until,
                        Repeat = t.Repeat
                    }).ToList();
                }
                

                var day = new DayViewModel(tasksForDay)
                {
                    Date = date,
                    IsCurrentMonth = this.firstDay.Month == this.firstDay.AddDays(i).Month
                };

                this.days.Add(day);
            }
        }
    }
}
