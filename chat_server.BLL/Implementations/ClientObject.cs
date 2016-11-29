using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using chat_server.BLL.Contracts;


namespace chat_server.BLL.Implementations
{
    public class ClientObject : IClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        IServerObject _serverObject;

        public ClientObject(TcpClient tcpClient, IServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            _serverObject = serverObject;
            _serverObject.AddConnection(this);
        }

        public void Process()
        {
            throw new NotImplementedException();
        }

        public string GetMessage()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
