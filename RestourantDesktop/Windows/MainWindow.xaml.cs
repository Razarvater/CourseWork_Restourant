using RestourantDesktop.DialogManager.Dialogs.AuthorizeDialog.Pages;
using RestourantDesktop.Windows.Pages.Orders;
using RestourantDesktop.Windows.Pages.ProductDishesManager;
using RestourantDesktop.Windows.Pages.RoleManager;
using RestourantDesktop.Windows.Pages.UserManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using WindowControllers;

namespace RestourantDesktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RestourantWindowTemplate
    {
        private Dictionary<string, MenuItem> pagesList = new Dictionary<string, MenuItem>();  

        public MainWindow()
        {
            OpenAuthMenu();
            UserController.UserController.AuthorizedUserStatsChangedEvent += PagesListChanged;
            pagesList.Add("Роли", new MenuItem("Роли", typeof(RoleManagerPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));
            pagesList.Add("Пользователи", new MenuItem("Пользователи", typeof(UserManagerPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));
            pagesList.Add("Кухня", new MenuItem("Кухня", typeof(ProductDishesManager), new Icon(this.GetType(), "Resources.Images.Admin.ico")));
            pagesList.Add("Авторизация", new MenuItem("Авторизация", typeof(MainPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));
            pagesList.Add("Заказы", new MenuItem("Заказы", typeof(Orders), new Icon(this.GetType(), "Resources.Images.Admin.ico")));
        }

        private void PagesListChanged(object sender, EventArgs e)
        {
            ObservableCollection<MenuItem> tempCollection = new ObservableCollection<MenuItem>();
            foreach (string item in UserController.UserController.AuthorizedUser.PagesListForUser)
            {
                if (pagesList.TryGetValue(item, out MenuItem pageValue))
                    tempCollection.Add(pageValue);
            }
            ObservableCollection<MenuItem> deleteList = new ObservableCollection<MenuItem>();
            foreach (MenuItem item in vm.TabsList)
            {
                if (!tempCollection.Contains(item))
                    deleteList.Add(item);
            }

            foreach (MenuItem item in deleteList)
            {
                if (vm.SelectedPage == item.PageContent)
                {
                    vm.lastSelected = null;
                    vm.SelectedPage = null;
                    item.IsSelected = false;
                }
                vm.TabsList.Remove(item);
            }

            foreach (var item in tempCollection)
            {
                if(!vm.TabsList.Contains(item))
                    vm.TabsList.Add(item);
            }
        }

        private void OpenAuthMenu()
        {
            vm.TabsList.Clear();
            vm.TabsList.Add(new MenuItem("Авторизация", typeof(MainPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));
        }
    }
}