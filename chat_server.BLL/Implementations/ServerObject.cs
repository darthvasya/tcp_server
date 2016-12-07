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
        List<Room> rooms = new List<Room>();

        public ServerObject()
        {
            rooms.Add(new Room());
        }

        public void AddConnection(ClientObject clientObject)
        {
            bool makeNewRoom = true;
            clients.Add(clientObject);

            foreach (Room room in rooms)
            {
                if (room.clients.Count == 1 || room.clients.Count == 0)
                {
                    room.AddConnection(clientObject);
                    makeNewRoom = false;
                    break;
                }
            }

            if (makeNewRoom)
            {
                Room newRoom = new Room();
                newRoom.AddConnection(clientObject);
                rooms.Add(newRoom);
            }
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
            string roomId = "";
            foreach (Room room in rooms)
            {
                foreach (ClientObject client in room.clients)
                    if (client.Id == id)
                        roomId = room.roomId;
            }

            Room roomCurrent = rooms.Where(p => p.roomId == roomId).FirstOrDefault();
            foreach (ClientObject client in roomCurrent.clients)
            {
                byte[] data = Encoding.UTF8.GetBytes(roomCurrent.roomId + ". " + message);
                if (client.Id != id)
                    client.Stream.Write(data, 0, data.Length);
            }

        }

        public void Disconnect()
        {
            //tcpListener.Stop();

            foreach (ClientObject client in clients)
            {
                client.Close();
            }
            
        }

    }
}
