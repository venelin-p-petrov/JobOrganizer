using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace JobOrganizer.Behavior
{
    public static class CommandsBehavior
    {
        private static void ExecuteClickCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UIElement;
            if (control == null)
            {
                return;
            }
            if ((e.NewValue != null) && (e.OldValue == null))
            {
                control.PointerPressed += (snd, args) =>
                {
                    var command = (snd as UIElement).GetValue(CommandsBehavior.ClickProperty) as ICommand;
                    command.Execute(args);
                };
            }
        }

        public static ICommand GetClick(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ClickProperty);
        }

        public static void SetClick(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ClickProperty, value);
        }

        // Using a DependencyProperty as the backing store for Click.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickProperty =
            DependencyProperty.RegisterAttached("Click",
                typeof(ICommand),
                typeof(CommandsBehavior),
                new PropertyMetadata(null, ExecuteClickCommand));
    }
}
