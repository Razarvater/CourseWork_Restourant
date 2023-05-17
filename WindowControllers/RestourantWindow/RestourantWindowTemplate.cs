using System;
using System.Windows;
using System.Windows.Input;

namespace WindowControllers
{
    /// <summary>
    /// Кастомное окно
    /// </summary>
    public class RestourantWindowTemplate : Window
    {
        public readonly ViewModelNavigation vm = new ViewModelNavigation();

        /// <summary>
        /// Конструктор
        /// </summary>
        public RestourantWindowTemplate()
        {
            InitializeComponent();
            this.DataContext = vm;
            this.StateChanged += this.RestourantWindowTemplate_StateChanged;
        }

        public void DragWindow()
        {
            if (vm.RealMode == WindowState.Maximized)
            { 
                Point screenPosition = this.PointToScreen(Mouse.GetPosition(this));
                vm.MinMaxWindowCommand?.Execute(screenPosition);
            }
        }

        /// <summary>
        /// Стартовые состояния
        /// </summary>
        private void InitializeComponent()
        {
            //Задание дефолтного стиля для окна
            try
            {
               DefaultStyleKeyProperty.OverrideMetadata(typeof(RestourantWindowTemplate), new FrameworkPropertyMetadata(typeof(RestourantWindowTemplate)));
            }
            catch (Exception) { }
        }

        private void RestourantWindowTemplate_StateChanged(object sender, EventArgs e)
        {
            if (vm.WindowStateM == WindowState.Maximized)
            {
                vm.WindowStateM = WindowState.Normal;
                vm.MaxWindowCommand?.Execute(null);
            }
        }
    }
}