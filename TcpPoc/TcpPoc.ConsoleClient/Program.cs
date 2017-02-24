using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
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

            //var assembly = Assembly.GetExecutingAssembly();
            //var resourceName = "TcpPoc.ConsoleClient.1.wav";
            //using (var stream = assembly.GetManifestResourceStream(resourceName))
            //{
            //    var buffer = new byte[1024];
            //    var actual = stream.Read(buffer, 0, 1024);
            //    Console.WriteLine(actual);
            //} ;
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
