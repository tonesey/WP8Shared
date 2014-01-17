using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System;

namespace Wp8Shared.UserControls
{
    public enum MsgboxMode
    {
        Ok,
        YesNo
    }

    public enum MsgboxResponse
    {
        Yes,
        No
    }

    public delegate void MsgboxClosedEventHandler(MsgboxResponse response);


    public partial class MyMsgboxContent
    {
        public static readonly DependencyProperty PageOrientationProperty =
            DependencyProperty.Register("PageOrientation",
                                        typeof(PageOrientation),
                                        typeof(MyMsgboxContent),
                                        new PropertyMetadata(PageOrientation.LandscapeLeft));

        public PageOrientation PageOrientation
        {
            set { SetValue(PageOrientationProperty, value); }
            get { return (PageOrientation)GetValue(PageOrientationProperty); }
        }

        public event MsgboxClosedEventHandler MsgboxClosedEvent;

        private readonly Popup _popup;

        public MsgboxResponse Response { get; private set; }

        MsgboxMode _mode = MsgboxMode.Ok;

        private string _text = string.Empty;

        Action<MsgboxResponse> _completed;

        // public MyMsgboxContent(Popup popup, MsgboxMode mode, string text, Action<MsgboxResponse> completed = null)
        public MyMsgboxContent(Popup popup, MsgboxMode mode, string text)
        {
            InitializeComponent();
            //LocalizeUI();
            DataContext = this;
            _popup = popup;
            _mode = mode;
            _text = text;
            // _completed = completed;

            ButtonNo.Visibility = Visibility.Collapsed;
            ButtonYes.Visibility = Visibility.Collapsed;
        }

        //private void LocalizeUI()
        //{
        //}

        private void MsgboxClosed(MsgboxResponse res)
        {
            if (_popup != null)
            {
                _popup.IsOpen = false;

                if (MsgboxClosedEvent != null)
                {
                    MsgboxClosedEvent(res);
                }
                //_completed(res);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockContent.Text = _text;
            ButtonYes.Visibility = Visibility.Visible;
            switch (_mode)
            {
                case MsgboxMode.Ok:
                    ButtonNo.Visibility = Visibility.Collapsed;
                    break;
                case MsgboxMode.YesNo:
                    ButtonNo.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        internal void Close()
        {
        }

        private void ButtonNo_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MsgboxClosed(MsgboxResponse.No);
        }

        private void ButtonYes_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MsgboxClosed(MsgboxResponse.Yes);
        }
    }
}