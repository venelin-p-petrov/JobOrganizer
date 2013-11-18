using System;
using System.Linq;
using JobOrganizer.Enumerations;

namespace JobOrganizer.Models
{
    public class TaskModel : IEquatable<TaskModel>
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime Until { get; set; }

        public RepeatInterval Repeat { get; set; }

        public bool Equals(TaskModel other)
        {
            bool equalStart = this.Start.Year == other.Start.Year &&
                this.Start.Month == other.Start.Month &&
                this.Start.Day == other.Start.Day &&
                this.Start.Hour == other.Start.Hour &&
                this.Start.Minute == other.Start.Minute;

            bool equalEnd = this.End.Year == other.End.Year &&
                this.End.Month == other.End.Month &&
                this.End.Day == other.End.Day &&
                this.End.Hour == other.End.Hour &&
                this.End.Minute == other.End.Minute;

            bool equalUntil = this.Until.Year == other.Until.Year &&
                this.Until.Month == other.Until.Month &&
                this.Until.Day == other.Until.Day &&
                this.Until.Hour == other.Until.Hour &&
                this.Until.Minute == other.Until.Minute;

            if (this.Title == other.Title &&
                this.Message == other.Message &&
                equalStart && equalEnd &&
                equalUntil && this.Repeat == other.Repeat)
            {
                return true;
            }

            return false;
        }
    }
}
