using RestourantDesktop.Database;
using System.Windows;

namespace RestourantDesktop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Exit += (sender, e) => Dependency.CloseApplication();
        }
    }
}
