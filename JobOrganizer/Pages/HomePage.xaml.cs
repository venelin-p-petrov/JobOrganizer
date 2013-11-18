using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using JobOrganizer.Data;
using JobOrganizer.Models;
using JobOrganizer.ViewModels;
using Windows.Foundation;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Printing;

namespace JobOrganizer.Pages
{
    public sealed partial class HomePage : JobOrganizer.Common.LayoutAwarePage
    {
        private DataTransferManager dataTransferManager;
        private PrintDocument document;
        private int pageCount;
        private Size? pageSize;
        private Rect? imageableRect;
        private const int RowCount = 10;
        private const int ColCount = 2;

        public HomePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareTasks);
            
            PrintManager manager = PrintManager.GetForCurrentView();

            manager.PrintTaskRequested += OnPrintTaskRequested;
        }

        void OnPrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        { 
            var deferral = args.Request.GetDeferral();

            PrintTask printTask = args.Request.CreatePrintTask(
                "Todays Tasks",
                OnPrintTaskSourceRequestedHandler);

            printTask.Completed += OnPrintTaskCompleted;

            deferral.Complete();
        }

        void OnPrintTaskCompleted(PrintTask sender, PrintTaskCompletedEventArgs args)
        {
            this.pageCount = 0;
            this.document = null;
        }

        async void OnPrintTaskSourceRequestedHandler(PrintTaskSourceRequestedArgs args)
        {
            var deferral = args.GetDeferral();

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    this.document = new PrintDocument();

                    this.document.Paginate += OnPaginate;
                    this.document.GetPreviewPage += OnGetPreviewPage;
                    this.document.AddPages += OnAddPages;

                    // Tell the caller about it.  
                    args.SetSource(this.document.DocumentSource);
                });
            deferral.Complete();
        }

        private void OnAddPages(object sender, AddPagesEventArgs e)
        {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    for (int i = 1; i <= this.pageCount; i++)
                    {
                        this.document.AddPage(this.BuildPage(i));
                    }

                    this.document.AddPagesComplete();  
                }); 
        }

        private void OnGetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    this.document.SetPreviewPage(e.PageNumber,
                        this.BuildPage(e.PageNumber));  
                });
        }

        UIElement BuildPage(int pageNumber)
        {
            // Account for pages going 1..N rather than 0..N-1  
            int pageIndex = pageNumber - 1;

            Grid parentGrid = new Grid();
            parentGrid.Width = this.pageSize.Value.Width;
            parentGrid.Height = this.pageSize.Value.Height;

            // Make a grid  
            Grid grid = new Grid();
            grid.Margin = new Thickness(100, 20, 100, 50);

            // Make grid rows and cols  
            GridLength length = new GridLength(1, GridUnitType.Star);

            parentGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            parentGrid.RowDefinitions.Add(new RowDefinition() { Height = length });
            parentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = length });

            for (int i = 0; i < RowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = length });
            }
            for (int i = 0; i < ColCount; i++)
            {
                if (i % 2 == 0)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
                }
                else
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = length });
                }
            }

            // Make items  

            TextBlock pageTitle = new TextBlock();
            pageTitle.Text = "Tasks for " + DateTime.Now.ToString("MM/dd/yyyy");
            pageTitle.Margin = new Thickness(100, 50, 100, 20);
            pageTitle.FontSize = 30;
            Grid.SetRow(pageTitle, 0);
            Grid.SetColumn(pageTitle, 0);
            parentGrid.Children.Add(pageTitle);

            var tasks = TaskRepository.Tasks.AsQueryable()
                    .OrderBy(t => t.Start)
                    .Where(t => t.Start >= DateTime.Now && t.Start < DateTime.Today.AddDays(1))
                    .Take(10);

            int rowNumber = 0;

            foreach (TaskModel task in tasks)
            {
                TextBlock timeText = new TextBlock();
                timeText.Text = task.Start.ToString("t");
                timeText.FontSize = 20;

                Grid.SetRow(timeText, rowNumber);
                Grid.SetColumn(timeText, 0);

                TextBlock titleText = new TextBlock();
                titleText.Text = task.Title;
                titleText.FontSize = 20;
                titleText.TextWrapping = TextWrapping.Wrap;

                Grid.SetRow(titleText, rowNumber);
                Grid.SetColumn(titleText, 1);

                rowNumber++;

                grid.Children.Add(timeText);
                grid.Children.Add(titleText);
            }

            // Offset it into the printable area  
            Canvas.SetLeft(grid, this.imageableRect.Value.Left);
            Canvas.SetTop(grid, this.imageableRect.Value.Top);

            Grid.SetRow(grid, 1);
            Grid.SetColumn(grid, 0);
            parentGrid.Children.Add(grid);

            return (parentGrid);
        } 

        private void OnPaginate(object sender, PaginateEventArgs e)
        {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    this.GetPageSize(e);
                    this.document.SetPreviewPageCount(1, PreviewPageCountType.Final);
                });
        }

        void GetPageSize(PaginateEventArgs e)
        {
            if (this.pageSize == null)
            {
                PrintPageDescription description = e.PrintTaskOptions.GetPageDescription(
                  (uint)e.CurrentPreviewPageNumber);

                this.pageSize = description.PageSize;
                this.imageableRect = description.ImageableRect;
            }
        }

        private async void ShareTasks(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;

            request.Data.Properties.Title = "Shared Tasks";
            request.Data.Properties.Description = "File containing information about all tasks.";

            DataRequestDeferral deferral = request.GetDeferral();

            try
            {
                var tempFolder = ApplicationData.Current.TemporaryFolder;
                var saveFile = await tempFolder.CreateFileAsync("TempSavedTasks.xml", CreationCollisionOption.ReplaceExisting);

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
                        List<IStorageItem> storageItems = new List<IStorageItem>();
                        storageItems.Add(saveFile);
                        request.Data.SetStorageItems(storageItems);
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
            catch
            {
            }
            finally
            {
                deferral.Complete();
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
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void MonthButtonClick(object sender, RoutedEventArgs e)
        {
            bottomAppBar.IsOpen = false;
            this.Frame.Navigate(typeof(MonthPage));
        }

        private void WeekButtonClick(object sender, RoutedEventArgs e)
        {
            bottomAppBar.IsOpen = false;
            this.Frame.Navigate(typeof(WeekPage));
        }

        private void TaskButtonClick(object sender, RoutedEventArgs e)
        {
            bottomAppBar.IsOpen = false;
            this.Frame.Navigate(typeof(TaskPage));
        }

        private void TaskClicked(object sender, PointerRoutedEventArgs e)
        {
            var task = (sender as FrameworkElement).DataContext as TaskViewModel;
            (this.DataContext as AppViewModel).SelectTask(task);
            this.Frame.Navigate(typeof(TaskPage));
        }
    }
}