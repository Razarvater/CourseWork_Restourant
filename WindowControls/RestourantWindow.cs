using System.Windows;

namespace WindowControls
{
    /// <summary>
    /// Кастомное окно
    /// </summary>
    public class RestourantWindow : Window
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RestourantWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Стартовые состояния
        /// </summary>
        private void InitializeComponent()
        {
            Title = "My Window";
            Width = 400;
            Height = 300;
        }
    }
}
