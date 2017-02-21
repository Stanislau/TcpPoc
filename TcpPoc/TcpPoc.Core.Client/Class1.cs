using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace TcpPoc.Core.Client
{
    public interface ILogger
    {
        void WriteLine(string message);
    }

    public class ClientWrapper : IDisposable
    {
        private readonly ILogger _console;
        private TcpSocketClient _client;

        public ClientWrapper(ILogger console)
        {
            _console = console;
            _client = new TcpSocketClient();
        }

        public async Task Run()
        {
            await _client.ConnectAsync("192.168.1.3", 4088);
            _console.WriteLine("client is connected");
            var buffer = new byte[1];
            _client.GetStream().Read(buffer, 0, 1);
            _console.WriteLine(buffer[0].ToString());
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
