using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RestourantDesktop.DialogManager.Dialogs.AuthorizeDialog.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }

        private void RoleName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(sender as DependencyObject), null);
            }
        }
    }
}
