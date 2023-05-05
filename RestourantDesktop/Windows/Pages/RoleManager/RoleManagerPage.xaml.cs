using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RestourantDesktop.Windows.Pages.RoleManager
{
    /// <summary>
    /// Логика взаимодействия для RoleManagerPage.xaml
    /// </summary>
    public partial class RoleManagerPage : UserControl
    {
        private ViewModelRoles vm;
        public RoleManagerPage()
        {
            InitializeComponent();
            vm = new ViewModelRoles();
            this.DataContext = vm;
        }

        /// <summary>
        /// Асинхронная инициализация списков в ViewModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PageLoaded(object sender, RoutedEventArgs e) =>
            await vm.InitVM();

        private void RoleName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Keyboard.ClearFocus();
        }
    }
}