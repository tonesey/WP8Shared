using System;
using System.Globalization;
using System.Windows.Data;


namespace Wp8Shared.Converters
{
    public class ProgBarConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bool.Parse(value.ToString()) ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}