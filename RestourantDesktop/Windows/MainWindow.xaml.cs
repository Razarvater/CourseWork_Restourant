using RestourantDesktop.Windows.Pages.RoleManager;
using RestourantDesktop.Windows.Pages.UserManager;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
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

            vm.TabsList.Add(new MenuItem("Роли", typeof(RoleManagerPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));
            vm.TabsList.Add(new MenuItem("Пользователи", typeof(UserManagerPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));

            vm.SavedMode = MenuMode.VerticalMenu;
        }
    }
}