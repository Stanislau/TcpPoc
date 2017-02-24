using System;
using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace TcpPoc.Core.Client
{
    public class ClientWrapper : IDisposable
    {
        private const int BufferSize = 20 * 20 * 2 * 2;
        private readonly ILogger _console;
        private TcpSocketClient _client;

        public ClientWrapper(ILogger console)
        {
            _console = console;
            _client = new TcpSocketClient();
        }

        public async Task Run()
        {
            await _client.ConnectAsync("13.81.65.60", 4088);
            _console.WriteLine("client is connected");

            try
            {
                var buffer = new byte[BufferSize];
                var offset = 0;
                var actuallyRead = 0;

                do
                {
                    actuallyRead = await _client.GetStream().ReadAsync(buffer, 0, BufferSize);
                    offset += actuallyRead;
                    _console.WriteLine(offset.ToString());

                } while (actuallyRead != 0);

                //_track.Stop();
                _console.WriteLine("Completed");
            }
            catch (Exception exception)
            {
                _console.WriteLine(exception.Message);
                throw;
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}