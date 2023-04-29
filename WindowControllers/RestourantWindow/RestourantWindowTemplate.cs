using System;
using System.Windows;
using System.Windows.Input;

namespace WindowControllers
{
    /// <summary>
    /// ��������� ����
    /// </summary>
    public class RestourantWindowTemplate : Window
    {
        public readonly ViewModel vm = new ViewModel();

        /// <summary>
        /// �����������
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
        /// ��������� ���������
        /// </summary>
        private void InitializeComponent()
        {
            //������� ���������� ����� ��� ����
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RestourantWindowTemplate), new FrameworkPropertyMetadata(typeof(RestourantWindowTemplate)));
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
