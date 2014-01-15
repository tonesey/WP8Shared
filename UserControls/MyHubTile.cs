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
using Microsoft.Phone.Controls;
using System.Collections.Generic;

namespace Wp8Shared.UserControls
{

    public enum Genre
    {
        Undefined,
        Enterteninment,
        KidsAndFamily,
        Music,
        Social
    }

    public class MyHubTile : HubTile
    {
        public List<string> SupportedCultures { get; set; }
        public Guid Guid { get; set; }
        public Genre Genre { get; set; }
    }
}
