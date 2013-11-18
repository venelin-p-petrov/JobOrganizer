using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using JobOrganizer.Enumerations;
using JobOrganizer.Models;
using Windows.Storage;

namespace JobOrganizer.Data
{
    public static class TaskRepository
    {
        private static List<TaskModel> tasks;

        public static List<TaskModel> Tasks
        {
            get
            {
                if (tasks == null)
                {
                    LoadTasks();
                }
                return tasks;
            }
            set
            {
                tasks = value;
            }
        }

        public static void AddTask(TaskModel task)
        {
            var current = task;

            switch (task.Repeat)
            {
                case RepeatInterval.Once:
                    {
                        tasks.Add(task);
                        break;
                    }
                case RepeatInterval.EveryDay:
                    {
                        var nextStart = current.Start.AddDays(1);
                        var nextEnd = current.End.AddDays(1);
                        do
                        {
                            tasks.Add(current);
                            current = new TaskModel()
                            {
                                Title = task.Title,
                                Message = task.Message,
                                Start = nextStart,
                                End = nextEnd,
                                Until = task.End,
                                Repeat = RepeatInterval.Once
                            };
                            nextStart = nextStart.AddDays(1);
                            nextEnd = nextEnd.AddDays(1);
                        } while (current.Start <= task.Until);
                        break;
                    }
                case RepeatInterval.WeekDay:
                    {
                        var nextStart = current.Start.AddDays(1);
                        var nextEnd = current.End.AddDays(1);
                        do
                        {
                            if (!(current.Start.DayOfWeek == DayOfWeek.Saturday ||
                                current.Start.DayOfWeek == DayOfWeek.Sunday))
                            {
                                tasks.Add(current);
                            }

                            current = new TaskModel()
                            {
                                Title = task.Title,
                                Message = task.Message,
                                Start = nextStart,
                                End = nextEnd,
                                Until = task.End,
                                Repeat = RepeatInterval.Once
                            };
                            nextStart = nextStart.AddDays(1);
                            nextEnd = nextEnd.AddDays(1);
                        } while (current.Start <= task.Until);
                        break;
                    }
                case RepeatInterval.Week:
                    {
                        var nextStart = current.Start.AddDays(7);
                        var nextEnd = current.End.AddDays(7);
                        do
                        {
                            tasks.Add(current);

                            current = new TaskModel()
                            {
                                Title = task.Title,
                                Message = task.Message,
                                Start = nextStart,
                                End = nextEnd,
                                Until = task.End,
                                Repeat = RepeatInterval.Once
                            };
                            nextStart = nextStart.AddDays(7);
                            nextEnd = nextEnd.AddDays(7);
                        } while (current.Start <= task.Until);
                        break;
                    }
                case RepeatInterval.Month:
                    {
                        var nextStart = current.Start.AddMonths(1);
                        var nextEnd = current.End.AddMonths(1);
                        do
                        {
                            tasks.Add(current);

                            current = new TaskModel()
                            {
                                Title = task.Title,
                                Message = task.Message,
                                Start = nextStart,
                                End = nextEnd,
                                Until = task.End,
                                Repeat = RepeatInterval.Once
                            };
                            nextStart = nextStart.AddMonths(1);
                            nextEnd = nextEnd.AddMonths(1);
                        } while (current.Start <= task.Until);
                        break;
                    }
                case RepeatInterval.Year:
                    {
                        var nextStart = current.Start.AddYears(1);
                        var nextEnd = current.End.AddYears(1);
                        do
                        {
                            tasks.Add(current);

                            current = new TaskModel()
                            {
                                Title = task.Title,
                                Message = task.Message,
                                Start = nextStart,
                                End = nextEnd,
                                Until = task.End,
                                Repeat = RepeatInterval.Once
                            };
                            nextStart = nextStart.AddYears(1);
                            nextEnd = nextEnd.AddYears(1);
                        } while (current.Start <= task.Until);
                        break;
                    }
                default:
                    break;
            }

            SaveTasks();
        }

        public static void RemoveTask(TaskModel task)
        {
            tasks.Remove(task);
            SaveTasks();
        }

        private static async void SaveTasks()
        {
            var roamingFolder = ApplicationData.Current.RoamingFolder;
            var file = await roamingFolder.CreateFileAsync("SavedTasks.xml", CreationCollisionOption.OpenIfExists);
            string text = string.Empty;

            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<TaskModel>));
                var writer = new StringWriter();
                xml.Serialize(writer, tasks);
                text = writer.ToString();

                await FileIO.WriteTextAsync(file, text);
            }
            catch
            {
            }
        }

        public static async void LoadTasks()
        {
            try
            {
                var roamingFolder = ApplicationData.Current.RoamingFolder;
                var file = await roamingFolder.GetFileAsync("SavedTasks.xml");

                string tasksAsXml = await FileIO.ReadTextAsync(file);

                XmlSerializer xml = new XmlSerializer(typeof(List<TaskModel>));
                XmlReader xmlRead = XmlReader.Create(new StringReader(tasksAsXml));
                tasks = (xml.Deserialize(xmlRead)) as List<TaskModel>;
                if (tasks == null)
                {
                    tasks = new List<TaskModel>();
                }
            }
            catch (Exception exc)
            {
                tasks = new List<TaskModel>();
            }
        }
    }
}
