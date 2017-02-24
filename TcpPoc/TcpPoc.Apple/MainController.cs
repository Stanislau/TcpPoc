using System;
using Cirrious.FluentLayouts.Touch;
using Sockets.Plugin;
using UIKit;

namespace TcpPoc.Apple
{
    public class MainController : UIViewController
    {
        private TcpSocketClient _client;
        private UILabel _status;
        private UILabel _message;

        private const int BufferSize = 20 * 20 * 2 * 2;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            var connect = new UIButton(UIButtonType.System);
            connect.SetTitle("Play", UIControlState.Normal);
            Add(connect);

            _status = new UILabel();
            _status.TextColor = UIColor.Black;
            _status.Text = "[Not changed]";
            Add(_status);

            _message = new UILabel();
            _message.TextColor = UIColor.Black;
            _message.Text = "[Not changed]";
            Add(_message);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                connect.CenterX().EqualTo().CenterXOf(View),
                connect.CenterY().EqualTo().CenterYOf(View),
                connect.Width().EqualTo().WidthOf(connect),
                connect.Height().EqualTo().HeightOf(connect),

                _message.Bottom().EqualTo().TopOf(connect),
                _message.Height().EqualTo().HeightOf(_message),
                _message.Width().EqualTo().WidthOf(_message),
                _message.CenterX().EqualTo().CenterXOf(View),

                _status.Bottom().EqualTo().TopOf(_message),
                _status.Height().EqualTo().HeightOf(_status),
                _status.Width().EqualTo().WidthOf(_status),
                _status.CenterX().EqualTo().CenterXOf(View)
                );

            _client = new TcpSocketClient();

            connect.TouchUpInside += ConnectOnTouchUpInside;
        }

        private async void ConnectOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            try
            {
                await _client.ConnectAsync("13.81.65.60", 4088);

                _status.Text = "Connected";
            }
            catch (Exception exception)
            {
                _status.Text = exception.Message;
                return;
            }

            try
            {
                var buffer = new byte[BufferSize];
                var offset = 0;
                var actuallyRead = 0;

                do
                {
                    actuallyRead = await _client.Socket.GetStream().ReadAsync(buffer, 0, BufferSize);
                    offset += actuallyRead;
                    _message.Text = offset.ToString();
                    //_track.Write(buffer, 0, actuallyRead);

                } while (actuallyRead != 0);

                //_track.Stop();
                _message.Text = "Completed";
            }
            catch (Exception exception)
            {
                _message.Text = exception.Message;
                throw;
            }
        }
    }
}