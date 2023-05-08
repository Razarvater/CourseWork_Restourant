using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WindowControllers
{
    /// <summary>
    /// Конвертер, будет-ли виден элемент или нет
    /// </summary>
    internal class ButtonVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            (WindowState)value is WindowState.Normal ? Visibility.Hidden : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    /// <summary>
    /// Конвертер, будет-ли видно меню
    /// </summary>
    internal class MenuVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    /// <summary>
    /// Конвертер, будет меню вертикальным или горизонтальным
    /// </summary>
    internal class MenuModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (Dock)value == Dock.Top ? Orientation.Horizontal : Orientation.Vertical;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    /// <summary>
    /// Конвертер, цветов
    /// </summary>
    internal class MenuColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            ((SolidColorBrush)value).Color;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}