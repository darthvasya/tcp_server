using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using chat_server.BLL.Contracts;

namespace chat_server.BLL.Implementations
{
    public class ServerObject : IServerObject
    {
        static TcpListener tcpListener;
        List<ClientObject> clients = new List<ClientObject>();

        public void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        public void RemoveConnection(string id)
        {
            ClientObject clientObjectToRemove = clients.Where(p => p.Id == id).FirstOrDefault();
            if (clientObjectToRemove != null)
                clients.Remove(clientObjectToRemove);
        }

        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8000); //localhost:8000
                tcpListener.Start();
                Console.WriteLine("Start listenning on port 8000");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        public void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (ClientObject client in clients)
            {
                if (client.Id != id)
                    client.Stream.Write(data, 0, data.Length);
            }
        }

        public void Disconnect()
        {
            tcpListener.Stop();

            foreach (ClientObject client in clients)
            //client.Close();
            {

            }
            
        }

    }
}
