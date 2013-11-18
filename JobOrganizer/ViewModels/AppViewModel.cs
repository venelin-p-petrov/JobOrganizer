using JobOrganizer.Behavior;
using JobOrganizer.Common;
using JobOrganizer.Enumerations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using JobOrganizer.Models;
using Windows.Storage;
using Windows.Storage.Pickers;
using JobOrganizer.Data;

namespace JobOrganizer.ViewModels
{
    public class AppViewModel : BindableBase
    {
        private MonthViewModel month;
        private WeekViewModel week;
        private TaskViewModel selectedTask;
        private ObservableCollection<TaskViewModel> todayTasks;
        private ObservableCollection<TaskViewModel> tomorrowTasks;
        private DateTime taskStartDate;
        private DateTime taskStartTime;
        private DateTime taskEndDate;
        private DateTime taskEndTime;
        private DateTime taskUntil;
        private string taskRepeat;
        private string taskTitle;
        private string taskMessage;
        private ICommand prevMonthCommand;
        private ICommand nextMonthCommand;
        private ICommand prevWeekCommand;
        private ICommand nextWeekCommand;
        private ICommand createTaskCommand;
        private ICommand cancelCreateTaskCommand;
        private ICommand saveTasksCommand;
        private ICommand loadTasksCommand;

        public MonthViewModel Month
        {
            get
            {
                return this.month;
            }
            set
            {
                this.month = value;
                this.OnPropertyChanged();
            }
        }

        public WeekViewModel Week
        {
            get
            {
                return this.week;
            }
            set
            {
                this.week = value;
                this.OnPropertyChanged();
            }
        }

        public IList<TaskViewModel> TodayTasks
        {
            get
            {
                if (this.todayTasks == null)
                {
                    this.todayTasks = new ObservableCollection<TaskViewModel>();
                }

                return this.todayTasks;
            }
            set
            {
                if (this.todayTasks == null)
                {
                    this.todayTasks = new ObservableCollection<TaskViewModel>();
                }
                this.SetObservableValues(this.todayTasks, value);
            }
        }

        public IList<TaskViewModel> TomorrowTasks
        {
            get
            {
                if (this.tomorrowTasks == null)
                {
                    this.tomorrowTasks = new ObservableCollection<TaskViewModel>();
                }

                return this.tomorrowTasks;
            }
            set
            {
                if (this.tomorrowTasks == null)
                {
                    this.tomorrowTasks = new ObservableCollection<TaskViewModel>();
                }
                this.SetObservableValues(this.tomorrowTasks, value);
            }
        }

        public DateTime TaskStartDate
        {
            get
            {
                if (this.taskStartDate == DateTime.MinValue)
                {
                    this.taskStartDate = DateTime.Now;
                }
                return this.taskStartDate;
            }
            set
            {
                this.taskStartDate = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime TaskStartTime
        {
            get
            {
                if (this.taskStartTime == DateTime.MinValue)
                {
                    this.taskStartTime = DateTime.Now;
                }
                return this.taskStartTime;
            }
            set
            {
                this.taskStartTime = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime TaskEndDate
        {
            get
            {
                if (this.taskEndDate == DateTime.MinValue)
                {
                    this.taskEndDate = DateTime.Now;
                }
                return this.taskEndDate;
            }
            set
            {
                this.taskEndDate = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime TaskEndTime
        {
            get
            {
                if (this.taskEndTime == DateTime.MinValue)
                {
                    this.taskEndTime = DateTime.Now;
                }
                return this.taskEndTime;
            }
            set
            {
                this.taskEndTime = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime TaskUntil
        {
            get
            {
                if (this.taskUntil == DateTime.MinValue)
                {
                    this.taskUntil = DateTime.Now;
                }
                return this.taskUntil;
            }
            set
            {
                this.taskUntil = value;
                this.OnPropertyChanged();
            }
        }

        public string TaskRepeat
        {
            get
            {
                if (string.IsNullOrEmpty(this.taskRepeat))
                {
                    this.taskRepeat = "Once";
                }
                return this.taskRepeat;
            }
            set
            {
                this.taskRepeat = value;
                this.OnPropertyChanged();
            }
        }

        public string TaskTitle
        {
            get
            {
                return this.taskTitle;
            }
            set
            {
                this.taskTitle = value;
                this.OnPropertyChanged();
            }
        }

        public string TaskMessage
        {
            get
            {
                return this.taskMessage;
            }
            set
            {
                this.taskMessage = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand PrevMonth
        {
            get
            {
                if (this.prevMonthCommand == null)
                {
                    this.prevMonthCommand = new DelegateCommand<object>(this.HandlePrevMonthCommand);
                }
                return this.prevMonthCommand;
            }
        }

        public ICommand NextMonth
        {
            get
            {
                if (this.nextMonthCommand == null)
                {
                    this.nextMonthCommand = new DelegateCommand<object>(this.HandleNextMonthCommand);
                }
                return this.nextMonthCommand;
            }
        }

        public ICommand PrevWeek
        {
            get
            {
                if (this.prevWeekCommand == null)
                {
                    this.prevWeekCommand = new DelegateCommand<object>(this.HandlePrevWeekCommand);
                }
                return this.prevWeekCommand;
            }
        }

        public ICommand NextWeek
        {
            get
            {
                if (this.nextWeekCommand == null)
                {
                    this.nextWeekCommand = new DelegateCommand<object>(this.HandleNextWeekCommand);
                }
                return this.nextWeekCommand;
            }
        }

        public ICommand CreateTask
        {
            get
            {
                if (this.createTaskCommand == null)
                {
                    this.createTaskCommand = new DelegateCommand<object>(this.HandleCreateTaskCommand);
                }
                return this.createTaskCommand;
            }
        }

        public ICommand CancelCreateTask
        {
            get
            {
                if (this.cancelCreateTaskCommand == null)
                {
                    this.cancelCreateTaskCommand = new DelegateCommand<object>(this.HandleCancelCreateTaskCommand);
                }
                return this.cancelCreateTaskCommand;
            }
        }

        public ICommand SaveTasksCommand
        {
            get
            {
                if (this.saveTasksCommand == null)
                {
                    this.saveTasksCommand = new DelegateCommand<object>(this.HandleSaveTasksCommand);
                }
                return this.saveTasksCommand;
            }
        }

        public ICommand LoadTasksCommand
        {
            get
            {
                if (this.loadTasksCommand == null)
                {
                    this.loadTasksCommand = new DelegateCommand<object>(this.HandleLoadTasksCommand);
                }
                return this.loadTasksCommand;
            }
        }

        public AppViewModel()
        {
            this.UpdateTasks();

                this.month = new MonthViewModel();
                this.week = new WeekViewModel();
        }

        public void UpdateViewModels()
        {
            this.Week = new WeekViewModel(this.week.FirstDay);
            this.Month = new MonthViewModel(this.month.FirstDay);
        }

        public void SelectTask(TaskViewModel task)
        {
            this.TaskTitle = task.Title;
            this.TaskMessage = task.Message;
            this.TaskStartDate = task.Start;
            this.TaskStartTime = task.Start;
            this.TaskEndDate = task.End;
            this.TaskEndTime = task.End;
            this.TaskUntil = task.Until;
            this.TaskRepeat = task.Repeat.ToString();
            this.selectedTask = task;
        }

        internal void HandlePrevMonthCommand(object parameter)
        {
            this.Month = new MonthViewModel(this.month.FirstDay.AddMonths(-1));
        }

        internal void HandleNextMonthCommand(object parameter)
        {
                this.Month = new MonthViewModel(this.month.FirstDay.AddMonths(1));
        }

        internal void HandlePrevWeekCommand(object parameter)
        {
                this.Week = new WeekViewModel(this.week.FirstDay.AddDays(-7));
        }

        internal void HandleNextWeekCommand(object parameter)
        {
                this.Week = new WeekViewModel(this.week.FirstDay.AddDays(7));
        }

        internal void HandleCreateTaskCommand(object parameter)
        {
            var task = new TaskModel()
            {
                Title = this.TaskTitle,
                Message = this.TaskMessage,
                Start = new DateTime(this.TaskStartDate.Year, this.TaskStartDate.Month, this.TaskStartDate.Day,
                    this.TaskStartTime.Hour, this.TaskStartTime.Minute, this.TaskStartTime.Second),
                End = new DateTime(this.TaskEndDate.Year, this.TaskEndDate.Month, this.TaskEndDate.Day,
                    this.TaskEndTime.Hour, this.TaskEndTime.Minute, this.TaskEndTime.Second),
                Until = new DateTime(this.TaskUntil.Year, this.TaskUntil.Month, this.TaskUntil.Day,
                    this.TaskEndTime.Hour, this.TaskEndTime.Minute, this.TaskEndTime.Second),
                Repeat = (RepeatInterval)Enum.Parse(typeof(RepeatInterval), this.TaskRepeat)
            };

            if (this.selectedTask == null)
            {
                TaskRepository.AddTask(task);
            }
            else
            {
                this.selectedTask.Title = task.Title;
                this.selectedTask.Message = task.Message;
                this.selectedTask.Start = task.Start;
                this.selectedTask.End = task.End;
                this.selectedTask.Repeat = task.Repeat;
            }

            this.TaskTitle = string.Empty;
            this.TaskMessage = string.Empty;

            UpdateTasks();

            this.selectedTask = null;
        }

        private void UpdateTasks()
        {
            if (TaskRepository.Tasks != null)
            {
                this.TodayTasks = TaskRepository.Tasks.AsQueryable()
                    .OrderBy(t => t.Start)
                    .Where(t => t.Start >= DateTime.Now && t.Start < DateTime.Today.AddDays(1))
                    .Take(10)
                    .Select(t => new TaskViewModel()
                    {
                        Title = t.Title,
                        Message = t.Message,
                        Start = t.Start,
                        End = t.End,
                        Until = t.Until,
                        Repeat = t.Repeat
                    }).ToList();

                this.TomorrowTasks = TaskRepository.Tasks.AsQueryable()
                    .OrderBy(t => t.Start)
                    .Where(t => t.Start >= DateTime.Today.AddDays(1) && t.Start < DateTime.Today.AddDays(2))
                    .Take(10)
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
        }

        internal void HandleCancelCreateTaskCommand(object parameter)
        {
            if (this.selectedTask != null)
            {
                TaskRepository.RemoveTask(new TaskModel() 
                {
                    Title = this.selectedTask.Title,
                    Message = this.selectedTask.Message,
                    Start = this.selectedTask.Start,
                    End = this.selectedTask.End,
                    Until = this.selectedTask.Until,
                    Repeat = this.selectedTask.Repeat
                });

                UpdateTasks();

                this.selectedTask = null;
            }

            this.TaskTitle = string.Empty;
            this.TaskMessage = string.Empty;
            this.TaskStartDate = DateTime.Now;
            this.TaskStartTime = DateTime.Now;
            this.TaskEndDate = DateTime.Now;
            this.TaskEndTime = DateTime.Now;
            this.TaskUntil = DateTime.Now;
            this.TaskRepeat = "Once";
        }

        internal async void HandleSaveTasksCommand(object parameter)
        {
            FileSavePicker savePicker = new FileSavePicker();

            var xmlFileTypes = new List<string>(new string[] { ".xml" });

            savePicker.FileTypeChoices.Add(
                new KeyValuePair<string, IList<string>>("XML File", xmlFileTypes)
                );

            savePicker.SuggestedFileName = "Tasks";
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            var saveFile = await savePicker.PickSaveFileAsync();

            if (saveFile != null)
            {
                bool exceptionOccured = false;
                try
                {
                    string text = string.Empty;

                    XmlSerializer xml = new XmlSerializer(typeof(List<TaskModel>));
                    var writer = new StringWriter();
                    xml.Serialize(writer, TaskRepository.Tasks);
                    text = writer.ToString();

                    await Windows.Storage.FileIO.WriteTextAsync(saveFile, text);
                    await new Windows.UI.Popups.MessageDialog("File Saved!").ShowAsync();
                }
                catch (Exception ex)
                {
                    exceptionOccured = true;
                }

                if (exceptionOccured)
                {
                    await new Windows.UI.Popups.MessageDialog("An error occured while saving the file. Please try again.").ShowAsync();
                }
            }
        }

        internal async void HandleLoadTasksCommand(object parameter)
        {
            FileOpenPicker openPicker = new FileOpenPicker();

            openPicker.FileTypeFilter.Add("*");

            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                bool exceptionOccured = false;
                try
                {
                    string tasksAsXml = await FileIO.ReadTextAsync(file);

                    XmlSerializer xml = new XmlSerializer(typeof(List<TaskModel>));
                    XmlReader xmlRead = XmlReader.Create(new StringReader(tasksAsXml));
                    var tasks = (xml.Deserialize(xmlRead)) as List<TaskModel>;

                    foreach (TaskModel task in tasks)
                    {
                        if (!TaskRepository.Tasks.Contains(task))
                        {
                            TaskRepository.AddTask(task);
                        }
                    }

                    this.UpdateTasks();
                }
                catch (Exception exc)
                {
                    exceptionOccured = true;
                }

                if (exceptionOccured)
                {
                    await new Windows.UI.Popups.MessageDialog("An error occured while opening the file. Please try again.").ShowAsync();
                }
            }
        }

        private void SetObservableValues<T>(ObservableCollection<T> observableCollection, IList<T> values)
        {
            if (observableCollection != values)
            {
                observableCollection.Clear();
                foreach (var item in values)
                {
                    observableCollection.Add(item);
                }
            }
        }
    }
}
