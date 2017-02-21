using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpPoc.Core.Client;

namespace TcpPoc.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client Started.");

            using (var client = new ClientWrapper(new Logger()))
            {
                client.Run();
                Console.ReadKey();
            }
        }
    }

    public class Logger : ILogger
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
