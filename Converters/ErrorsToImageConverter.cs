using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;


namespace ScaleFinderWP7.Converters
{
    public class ErrorsToImageConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? new BitmapImage(new Uri("/ScaleFinderWP7;component/Resources/check.png", UriKind.Relative)) : new BitmapImage(new Uri("/ScaleFinderWP7;component/Resources/delete.png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}