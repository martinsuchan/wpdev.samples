using System;
using System.Globalization;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Windows;
using System.Windows.Data;
#endif

namespace WinSample.Converters
{
    /// <summary>
    /// Returns <see cref="Visibility.Visible"/>, if the input value is true and 
    /// <see cref="Visibility.Collapsed"/>, if the input value is false.
    /// </summary>
    internal class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// If set to true, the converter returns inverted results.
        /// </summary>
        public bool VisibleWhenFalse { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return DependencyProperty.UnsetValue;
            bool visible = ((bool)value) ^ VisibleWhenFalse;

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}