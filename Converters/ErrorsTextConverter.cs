using System;
using System.Globalization;
using System.Windows.Data;


namespace ScaleFinderWP7.Converters
{
    public class ErrorsTextConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse(value.ToString()) == 0)
            {
                return "no errors";
            }
            if (int.Parse(value.ToString()) == 1)
            {
                return "one error";
            }
            return string.Format("{0} errors", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}