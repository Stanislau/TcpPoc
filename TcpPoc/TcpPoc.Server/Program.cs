using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpPoc.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Started.");

            //Do().Wait();

            //Console.ReadKey();

            using (var server = new Server())
            {
                server.Run();
                Console.ReadKey();
            }
        }

        private static async Task Do()
        {
            try
            {
                var file = File.OpenRead("1.mp3");
                var position = 0;
                var buffer = new byte[1024];
                while (position < file.Length)
                {
                    position += await file.ReadAsync(buffer, 0, buffer.Length);
                    Console.WriteLine(position);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            
        }
    }
}
