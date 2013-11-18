using JobOrganizer.Common;
using System;
using System.Collections.Generic;

namespace JobOrganizer.ViewModels
{
    public class HourViewModel : BindableBase
    {
        public IList<TaskViewModel> Tasks { get; set; }

        public DateTime Time { get; set; }

        public string TimeString
        {
            get
            {
                return this.Time.ToString("hh tt");
            }
        }
    }
}
