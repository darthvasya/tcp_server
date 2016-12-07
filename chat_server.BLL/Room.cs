using chat_server.BLL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.BLL
{
    public class Room
    {
        public string roomId { get; private set; }
        public List<ClientObject> clients { get; private set; }

        public Room()
        {
            roomId = Guid.NewGuid().ToString();
            clients = new List<ClientObject>();
        }

        public bool CheckRoom()
        {
            if (clients.Count < 2)
                return true;
            else
                return false;
        }

        public void AddConnection(ClientObject client)
        {
            clients.Add(client);
        }
    }
}
