using RestourantDesktop.DialogManager.Dialogs.AuthorizeDialog.Pages;
using RestourantDesktop.Windows.Pages.RoleManager;
using RestourantDesktop.Windows.Pages.UserManager;
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
        public MainWindow()
        {
            OpenAuthMenu();
            Auth.authEvent += UserAuth;
            //UserAuth(null, true);
        }

        private void OpenAuthMenu()
        {
            vm.TabsList.Clear();
            vm.TabsList.Add(new MenuItem("Авторизация", typeof(MainPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));

            vm.SavedMode = MenuMode.None;
        }

        private void UserAuth(object a, bool e)
        {
            if(!e) this.Close();

            vm.TabsList.Clear();
            vm.SelectedPage = null;
            vm.TabsList.Add(new MenuItem("Роли", typeof(RoleManagerPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));
            vm.TabsList.Add(new MenuItem("Пользователи", typeof(UserManagerPage), new Icon(this.GetType(), "Resources.Images.Admin.ico")));

            vm.SavedMode = MenuMode.VerticalMenu;
        }
    }
}