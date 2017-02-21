using System;
using System.IO;
using System.Net.Sockets;
using Android.App;
using Android.Media;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace TcpPoc.Droid
{
    [Activity(Label = "TcpPoc.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const int BufferSize = 20 * 20 * 2 * 2;
        private TextView _connectionStatus;
        private TextView _message;
        private Button _connectButton;
        private AudioTrack _track;
        private TcpSocketClient _client;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            _track = new AudioTrack(Android.Media.Stream.Music, 8000, ChannelOut.Stereo, Encoding.Pcm16bit, BufferSize, AudioTrackMode.Stream, 42);
            _connectionStatus = FindViewById<TextView>(Resource.Id.connectionStatus);
            _message = FindViewById<TextView>(Resource.Id.message);
            _connectButton = FindViewById<Button>(Resource.Id.connectButton);

            _connectButton.Click += ConnectButtonOnClick;

        }

        private async void ConnectButtonOnClick(object sender, EventArgs eventArgs)
        {
            _client = new TcpSocketClient();
            await _client.ConnectAsync("192.168.1.3", 4088);
            _connectionStatus.Text = "Connected";
            _track.Play();

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
                    _track.Write(buffer, 0, actuallyRead);

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

