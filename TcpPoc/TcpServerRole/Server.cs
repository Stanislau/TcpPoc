using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

namespace TcpServerRole
{
    class Server : IDisposable
    {
        private TcpListener _tcpServer;

        private readonly List<TcpClient> _clients = new List<TcpClient>();

        private const int BufferSize = 20 * 20 * 2 * 2;

        public async Task Run(CancellationToken cancellationToken)
        {
            _tcpServer = new TcpListener(RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["ServerEndpoint"].IPEndpoint);
            _tcpServer.Start();

            await Task.Run(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    var tcpClient = await _tcpServer.AcceptTcpClientAsync();
                    _clients.Add(tcpClient);
                    Trace.TraceInformation("Client is connected");

                    var file = Assembly.GetAssembly(GetType()).GetManifestResourceStream("TcpServerRole.1.wav");

                    var offset = 0;
                    var buffer = new byte[BufferSize];
                    file.Position = 0;
                    while (offset < file.Length)
                    {
                        var actuallyRead = await file.ReadAsync(buffer, 0, BufferSize);
                        tcpClient.GetStream().Write(buffer, 0, buffer.Length);
                        offset += actuallyRead;
                    }

                    Trace.TraceInformation("File has been sent");
                }
            });
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