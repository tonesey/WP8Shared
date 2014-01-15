using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Wp8Shared.UserControls
{


    public class MyMsgbox
    {
        static Popup _popup = null;
        static MyMsgboxContent _popupChild = null;
        //static Delegate _act = null;
        static Page _parent;

        public static event MsgboxClosedEventHandler MsgboxClosedEvent;

        //public static void Show(Page parent, PageOrientation orientation, string text, Delegate act)
        public static void Show(Page parent, PageOrientation orientation, string text)
        {
            _parent = parent;
            //_act = act;
            _popup = new Popup();
            _popupChild = new MyMsgboxContent(_popup, MsgboxMode.Ok, text);
            _popupChild.Height = 400;
            _popupChild.Width = 400;
            _popupChild.PageOrientation = orientation;
            _popup.Child = _popupChild;
            _popup.VerticalOffset = 200;
            _popup.HorizontalOffset = 30;
            _popup.IsOpen = true;
            _popupChild.MsgboxClosedEvent -= _popupChild_MsgboxClosedEvent;
            _popupChild.MsgboxClosedEvent += _popupChild_MsgboxClosedEvent;
            _parent.IsEnabled = false;
        }

        static void _popupChild_MsgboxClosedEvent(MsgboxResponse response)
        {
            _parent.IsEnabled = true;
            if (MsgboxClosedEvent != null) MsgboxClosedEvent(response);
        }

      
    }
}
