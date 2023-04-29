using RestourantDesktop.Windows.Pages.RoleManager;
using System.Collections.ObjectModel;
using WindowControllers;

namespace RestourantDesktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RestourantWindowTemplate
    {
        public MainWindow()
        {
            //Пример заполнения
            ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();
            menuItems.Add(new MenuItem("Роли пользователей", typeof(RoleManagerPage)));
            menuItems.Add(new MenuItem("Роли пользователей", typeof(RoleManagerPage)));
            menuItems.Add(new MenuItem("Роли пользователей", typeof(RoleManagerPage)));

            vm.TabsList.Add(new MenuItem("Роли пользователей", typeof(RoleManagerPage)));
            vm.TabsList.Add(new MenuItem("Админка", menuItems));
        }
    }
}