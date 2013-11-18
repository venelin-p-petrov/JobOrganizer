using System;
using System.Collections.Generic;
using System.Linq;
using JobOrganizer.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace JobOrganizer.Pages
{
    public sealed partial class TaskPage : JobOrganizer.Common.LayoutAwarePage
    {
        public TaskPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is TaskViewModel)
            {
                (this.DataContext as AppViewModel).SelectTask(e.Parameter as TaskViewModel);
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            
            if (pageState != null)
            {
                if (pageState.ContainsKey("PickStartDate"))
                {
                    if (PickStartDate != null)
                    {
                        PickStartDate.Value = pageState["PickStartDate"] as DateTime?;
                    }
                    else
                    {
                        CollapsedPickStartDate.Value = pageState["PickStartDate"] as DateTime?;
                    }
                }
                if (pageState.ContainsKey("PickStartTime"))
                {
                    if (PickStartTime != null)
                    {
                        PickStartTime.Value = pageState["PickStartDate"] as DateTime?;
                    }
                    else
                    {
                        CollapsedPickStartTime.Value = pageState["PickStartDate"] as DateTime?;
                    }
                }
                if (pageState.ContainsKey("PickEndDate"))
                {
                    if (PickEndDate != null)
                    {
                        PickEndDate.Value = pageState["PickStartDate"] as DateTime?;
                    }
                    else
                    {
                        CollapsedPickEndDate.Value = pageState["PickStartDate"] as DateTime?;
                    }
                }
                if (pageState.ContainsKey("PickEndTime"))
                {
                    if (PickEndTime != null)
                    {
                        PickEndTime.Value = pageState["PickStartDate"] as DateTime?;
                    }
                    else
                    {
                        CollapsedPickEndTime.Value = pageState["PickStartDate"] as DateTime?;
                    }
                }
                if (pageState.ContainsKey("PickRepeat"))
                {
                    if (PickRepeat != null)
                    {
                        PickRepeat.SelectedItem = pageState["PickRepeat"];
                    }
                    else
                    {
                        CollapsedPickRepeat.SelectedItem = pageState["PickRepeat"];
                    }
                }
                if (pageState.ContainsKey("TaskTitle"))
                {
                    if (TaskTitle != null)
                    {
                        TaskTitle.Text = pageState["TaskTitle"] as string;
                    }
                    else
                    {
                        CollapsedTaskTitle.Text = pageState["TaskTitle"] as string;
                    }
                }
                if (pageState.ContainsKey("TaskMessage"))
                {
                    if (TaskMessage != null)
                    {
                        TaskMessage.Text = pageState["TaskMessage"] as string;
                    }
                    else
                    {
                        CollapsedTaskMessage.Text = pageState["TaskMessage"] as string;
                    }
                }
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            pageState["PickStartDate"] = PickStartDate != null ? PickStartDate.Value : CollapsedPickStartDate.Value;
            pageState["PickStartTime"] = PickStartTime != null ? PickStartTime.Value : CollapsedPickStartTime.Value;
            pageState["PickEndDate"] = PickEndDate != null ? PickEndDate.Value : CollapsedPickEndDate.Value;
            pageState["PickEndTime"] = PickEndTime != null ? PickEndTime.Value : CollapsedPickEndTime.Value;
            pageState["PickRepeat"] = PickRepeat != null ? PickRepeat.SelectedItem : CollapsedPickRepeat.SelectedItem;
            pageState["TaskTitle"] = TaskTitle != null ? TaskTitle.Text : CollapsedTaskTitle.Text;
            pageState["TaskMessage"] = TaskMessage != null ? TaskMessage.Text : CollapsedTaskMessage.Text;
        }

        private void CancelCreateTask(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
