using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Phone.Controls;


namespace Wp8Shared.Converters
{
    public class OrientationConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            var or = (PageOrientation)Enum.Parse(typeof(PageOrientation), value.ToString(), true);
            switch (or)
            {
                case PageOrientation.LandscapeLeft:
                    return 90;
                case PageOrientation.LandscapeRight:
                    return -90;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return value;
        }
        #endregion
    }
}