using System.Windows.Controls;

namespace WindowControllers.RestourantWindow.MenuVariants
{
    /// <summary>
    /// Логика взаимодействия для MenuVertical.xaml
    /// </summary>
    public partial class MenuVertical : UserControl
    {
        public MenuVertical(ViewModelNavigation vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
