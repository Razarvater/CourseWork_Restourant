using System.Windows;

namespace WindowControls
{
    /// <summary>
    /// ��������� ����
    /// </summary>
    public class RestourantWindow : Window
    {
        /// <summary>
        /// �����������
        /// </summary>
        public RestourantWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��������� ���������
        /// </summary>
        private void InitializeComponent()
        {
            Title = "My Window";
            Width = 400;
            Height = 300;
        }
    }
}
