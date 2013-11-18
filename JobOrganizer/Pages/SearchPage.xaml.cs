using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace JobOrganizer.Pages
{
    public sealed partial class SearchPage : JobOrganizer.Common.LayoutAwarePage
    {
        ViewModels.SearchViewModel currentViewModel = null;

        public SearchPage()
        {
            this.InitializeComponent();

            this.currentViewModel = new ViewModels.SearchViewModel();
            this.DataContext = this.currentViewModel;
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
            var queryText = navigationParameter as String;

            this.currentViewModel.QueryText = queryText;
        }

        private void ResultTaskClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(TaskPage), e.ClickedItem);
        }
    }
}
