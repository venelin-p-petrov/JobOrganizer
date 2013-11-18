using JobOrganizer.Common;
using JobOrganizer.Enumerations;
using System;
using System.Linq;

namespace JobOrganizer.ViewModels
{
    public class TaskViewModel : BindableBase
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime Until { get; set; }

        public RepeatInterval Repeat { get; set; }

        public string StartString
        {
            get
            {
                return this.Start.ToString("t");
            }
        }
    }
}
