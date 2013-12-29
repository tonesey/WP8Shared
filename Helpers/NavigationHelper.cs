using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Wp8Shared.Helpers
{
    public class NavigationHelper
    {
        public static void SafeNavigateTo(NavigationService ns, Dispatcher ds, string pageUrl)
        {
            ds.BeginInvoke(() =>
            {
                try
                {
                    ns.Navigate(new Uri(pageUrl, UriKind.Relative));
                }
                catch (InvalidOperationException)
                {
                }
            });
        }
    }
}
