using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RestourantDesktop.Windows.Pages.UserManager
{
    /// <summary>
    /// Логика взаимодействия для UserManagerPage.xaml
    /// </summary>
    public partial class UserManagerPage : UserControl
    {
        private ViewModelUsers vm;
        public UserManagerPage()
        {
            InitializeComponent();
            vm = new ViewModelUsers();
            this.DataContext = vm;
        }

        /// <summary>
        /// Асинхронная инициализация списков в ViewModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PageLoaded(object sender, RoutedEventArgs e) =>
            await vm.InitVM();

        private void TextBoxKeyValidator(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(sender as DependencyObject), null);
            }
        }
    }
}
