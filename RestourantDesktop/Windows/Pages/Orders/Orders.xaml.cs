using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RestourantDesktop.Windows.Pages.Orders
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : UserControl
    {
        private ViewModel vm;
        public Orders()
        {
            InitializeComponent();
            vm = new ViewModel();
            this.DataContext = vm;
        }

        private void TextBoxKeyValidator(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(sender as DependencyObject), null);
            }
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            await vm.InitModel();
        }
    }
}
