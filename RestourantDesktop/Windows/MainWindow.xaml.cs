using RestourantDesktop.Windows.Pages.RoleManager;
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

            vm.TabsList.Add(new MenuItem("Админ Панель", typeof(RoleManagerPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));
            //vm.TabsList.Add(new MenuItem("Админ Панель_2", typeof(RoleManagerPage)));
            //vm.TabsList.Add(new MenuItem("Админ Панель_3", typeof(RoleManagerPage)));
            //vm.TabsList.Add(new MenuItem("Админ Панель_4", typeof(RoleManagerPage)));

            vm.SavedMode = MenuMode.VerticalMenu;
        }
    }
}