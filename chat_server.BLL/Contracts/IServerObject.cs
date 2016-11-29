using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chat_server.BLL.Implementations;

namespace chat_server.BLL.Contracts
{
    public interface IServerObject
    {
        void AddConnection(ClientObject clientObject);
        void RemoveConnection(string id);
        void Listen();
        void BroadcastMessage(string message, string id);
        void Disconnect();
    }
}
