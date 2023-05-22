using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager
{
    /// <summary>
    /// Логика взаимодействия для ProductDishesManager.xaml
    /// </summary>
    public partial class ProductDishesManager : UserControl
    {
        private ViewModel vm;
        public ProductDishesManager()
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
    }
}
