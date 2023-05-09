using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowControllers.Themes
{
    /// <summary>
    /// Логика взаимодействия для BlackComboBox.xaml
    /// </summary>
    public partial class BlackComboBox : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(BlackComboBox), new UIPropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(BlackComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public BlackComboBox()
        {
            InitializeComponent();
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BlackComboBox control = d as BlackComboBox;
            ((control.ButtonSelect.Content as Grid).FindName("SelectedValue") as TextBlock).Text = e.NewValue?.ToString() ?? "";
        }

        private bool popIsOpened = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (popIsOpened)
            {
                ((ButtonSelect.Content as Grid).FindName("Bar") as TextBlock).Text = "Ʌ";
                popList.IsOpen = false;
                popIsOpened = false;
            }
            else if (!popIsOpened)
            {
                ((ButtonSelect.Content as Grid).FindName("Bar") as TextBlock).Text = "V";
                popList.IsOpen = true;
                popIsOpened = true;
            }
        }

        private void popList_Closed(object sender, EventArgs e)
        {
            // Popup закрыт по клику вне окна?
            if (!ButtonSelect.IsKeyboardFocused || !ButtonSelect.IsMouseOver)
                Button_Click(null, null);
        }
    }
}