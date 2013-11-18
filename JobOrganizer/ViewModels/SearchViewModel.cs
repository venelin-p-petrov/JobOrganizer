using System.Collections.ObjectModel;
using JobOrganizer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using JobOrganizer.Data;
using JobOrganizer.Models;

namespace JobOrganizer.ViewModels
{
    public class SearchViewModel : BindableBase
    {
        private string queryText = string.Empty;
        private ObservableCollection<TaskViewModel> results = new ObservableCollection<TaskViewModel>();

        public string QueryText
        {
            get
            {
                return this.queryText;
            }
            set
            {
                this.queryText = value;
                this.OnPropertyChanged();
                this.LoadResults();
            }
        }

        public IList<TaskViewModel> Results
        {
            get
            {
                if (this.results == null)
                {
                    this.results = new ObservableCollection<TaskViewModel>();
                }
                return this.results;
            }
            set
            {
                if (this.results == null)
                {
                    this.results = new ObservableCollection<TaskViewModel>();
                }

                this.results.Clear();

                foreach (var item in value)
                {
                    this.results.Add(item);
                }
            }
        }

        private void LoadResults()
        {
            var taskModels = TaskRepository.Tasks;

            foreach (TaskModel model in taskModels)
            {
                if (model.Title.ToLower().Contains(this.queryText))
                {
                    this.results.Add(new TaskViewModel()
                        {
                            Title = model.Title,
                            Message = model.Message,
                            Start = model.Start,
                            End = model.End,
                            Until = model.Until,
                            Repeat = model.Repeat
                        });
                }
            }
        }
    }
}
