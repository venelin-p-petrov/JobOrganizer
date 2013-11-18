using JobOrganizer.Common;
using System;
using Windows.UI;

namespace JobOrganizer.ViewModels
{
    public class CategoryViewModel : BindableBase
    {
        public string Title { get; set; }

        public Color Color { get; set; }
    }
}
