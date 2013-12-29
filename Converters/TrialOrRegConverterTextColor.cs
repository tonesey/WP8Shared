using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace ScaleFinderWP7.Converters
{
    public class TrialOrRegConverterTextColor : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                //available in trial
                return new SolidColorBrush(Colors.White);
            }
            //available in registered version
            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}