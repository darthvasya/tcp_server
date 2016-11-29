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
            try
            {
                Stream = client.GetStream();
                string message = GetMessage();
                userName = message;

                message = userName + " enter to the chat!";
                _serverObject.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);

                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = String.Format("{0}: {1}", userName, message);
                        Console.WriteLine(message);
                        _serverObject.BroadcastMessage(message, this.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        _serverObject.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _serverObject.RemoveConnection(this.Id);
                Close();
            }
        }

        public string GetMessage()
        {
            byte[] data = new byte[256];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;

            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        public void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
