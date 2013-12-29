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

namespace Wp8Shared.Helpers
{
    public class LogHelper
    {

        public static void Log(string data)
        {
#if DEBUG
            string str = string.Format("[{0}]\t{1}", new[] { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), data });
            Console.WriteLine(str);
#endif
        }
    }
}
