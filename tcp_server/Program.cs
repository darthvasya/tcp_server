using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using chat_server.BLL.Contracts;
using chat_server.BLL.Implementations;
namespace tcp_server
{
    class Program
    {
        static IServerObject server;
        static Thread listenThread;

        static void Main(string[] args)
        {
            try
            {
                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
