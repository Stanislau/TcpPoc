using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TcpPoc.Server
{
    class Server : IDisposable
    {
        private TcpListener _tcpServer;

        private List<TcpClient> _clients = new List<TcpClient>();

        private const int BufferSize = 20 * 20 * 2 * 2;

        public async Task Run()
        {
            var ip = GetIpAddress();
            Console.WriteLine("IP address is {0}", ip);
            _tcpServer = new TcpListener(IPAddress.Parse(ip), 4088);
            _tcpServer.Start();

            await Task.Run(async () =>
            {
                while (true)
                {
                    var tcpClient = await _tcpServer.AcceptTcpClientAsync();
                    Console.WriteLine("Client connected");
                    _clients.Add(tcpClient);

                    var file = File.OpenRead("1.wav");

                    var offset = 0;
                    var buffer = new byte[BufferSize];
                    file.Position = 0;
                    while (offset < file.Length)
                    {
                        Console.WriteLine("Offset is " + offset);
                        var actuallyRead = await file.ReadAsync(buffer, 0, BufferSize);
                        tcpClient.GetStream().Write(buffer, 0, buffer.Length);
                        offset += actuallyRead;
                    }
                }
            });
        }

        string GetIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }

        public void Dispose()
        {
            _tcpServer.Stop();
            _tcpServer = null;
            foreach (var tcpClient in _clients)
            {
                tcpClient.Close();
            }
        }
    }
}