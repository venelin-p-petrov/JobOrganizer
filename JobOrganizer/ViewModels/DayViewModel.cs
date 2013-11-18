using JobOrganizer.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JobOrganizer.Data;

namespace JobOrganizer.ViewModels
{
    public class DayViewModel : BindableBase
    {
        private DateTime date;
        private IList<TaskViewModel> tasks;
        private ObservableCollection<HourViewModel> hours;

        public DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
                this.OnPropertyChanged();
            }
        }

        public string DateString
        {
            get
            {
                return this.date.ToString("ddd, dd MMM");
            }
        }

        public bool IsCurrentMonth { get; set; }

        public IList<HourViewModel> Hours
        {
            get
            {
                if (this.hours == null)
                {
                    this.hours = new ObservableCollection<HourViewModel>();
                }
                this.hours.Clear();
                for (int i = 0; i < 24; i++)
                {
                    var time = this.Date.AddHours(i);
                    var hour = new HourViewModel()
                    {
                        Time = time,
                        Tasks = TaskRepository.Tasks.AsQueryable()
                            .Where(t => t.Start >= time && t.Start < time.AddHours(1))
                            .Select(t => new TaskViewModel()
                            {
                                Title = t.Title,
                                Message = t.Message,
                                Start = t.Start,
                                End = t.End,
                                Until = t.Until,
                                Repeat = t.Repeat
                            }).ToList()
                    };

                    this.hours.Add(hour);
                }

                return hours;
            }
        }

        public TaskViewModel TopTask
        {
            get
            {
                var task = this.tasks.AsQueryable().OrderBy(t => t.Start).First();
                return task;
            }
        }

        public bool HasTasks
        {
            get
            {
                return this.tasks.Count() > 0;
            }
        }

        public DayViewModel()
            : this(new List<TaskViewModel>())
        {
        }

        public DayViewModel(IList<TaskViewModel> tasks)
        {
            this.tasks = tasks;
        }
    }
}
