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
        static Page _parent;
        static MyMsgboxContent _popupChild = null;
        static Action<MsgboxResponse> _completed;

        public static void Show(PhoneApplicationPage parent, 
                                MsgboxMode mode, 
                                string text, 
                                Action<MsgboxResponse> completed = null,
                                int width = 400, int heigth = 400)
        {
            _parent = parent;
            _completed = completed;
            _popup = new Popup();

            _popupChild = new MyMsgboxContent(_popup, mode, text);
            _popupChild.Height = heigth;
            _popupChild.Width = width;
            _popupChild.PageOrientation = parent.Orientation;

            _popup.Child = _popupChild;

            switch (parent.Orientation)
            {
                case PageOrientation.Landscape:
                case PageOrientation.LandscapeLeft:
                case PageOrientation.LandscapeRight:
                    _popup.VerticalOffset = (parent.ActualWidth - _popupChild.Width) / 2;
                    _popup.HorizontalOffset = (parent.ActualHeight - _popupChild.Height) / 2;
                    break;
                case PageOrientation.Portrait:
                case PageOrientation.PortraitDown:
                case PageOrientation.PortraitUp:
                    _popup.VerticalOffset = (parent.ActualHeight - _popupChild.Height) / 2;
                    _popup.HorizontalOffset = (parent.ActualWidth - _popupChild.Width) / 2;
                    break;
                default:
                    break;
            }

            _popup.IsOpen = true;
            _popupChild.MsgboxClosedEvent -= _popupChild_MsgboxClosedEvent;
            _popupChild.MsgboxClosedEvent += _popupChild_MsgboxClosedEvent;
            _parent.IsEnabled = false;
        }

        static void _popupChild_MsgboxClosedEvent(MsgboxResponse response)
        {
            _parent.IsEnabled = true;
            if (_completed != null)
            {
                _completed(response);
            }
        }
    }
}
