using System.Windows;
using System.Windows.Input;

namespace WindowControllers.Themes
{
    public partial class Generic : ResourceDictionary
    {
        private void DragMove_Move(object sender, MouseButtonEventArgs e)
        {
            Window window = Window.GetWindow(sender as FrameworkElement);
            (window as RestourantWindowTemplate)?.DragWindow();

            window?.DragMove();
        }
    }
}
