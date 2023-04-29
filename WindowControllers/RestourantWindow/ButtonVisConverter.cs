using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace WindowControllers
{
    /// <summary>
    /// Конвертер, будетли виден элемент или нет
    /// </summary>
    internal class ButtonVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            (WindowState)value is WindowState.Normal ? Visibility.Hidden : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
