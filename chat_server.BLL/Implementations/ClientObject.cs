using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using chat_server.BLL.Contracts;
using System.IO;
using tank_game.Model;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

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
                Console.WriteLine("Enter to the process");
                Stream = client.GetStream();
                string message = GetMessage();
                userName = message;

                message = userName + " enter to the chat!";
                _serverObject.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);
                //TankInfo message = new TankInfo();

                while (true)
                {
                    try
                    {
                        //десерилиозовать в методе получения
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
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;

            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                //Console.WriteLine(bytes);
            }
            while (Stream.DataAvailable);

            //Console.WriteLine("All: " + data.Length);
            //File.Delete("tank.dat");

            //using (FileStream fstream = new FileStream("tank.dat", FileMode.OpenOrCreate))
            //{
            //    // преобразуем строку в байты
            //    //byte[] array = System.Text.Encoding.Default.GetBytes(text);
            //    // запись массива байтов в файл
            //    fstream.Write(data, 0, data.Length);
            //    Console.WriteLine("Текст записан в файл");
            //}

            ////BinaryFormatter formatter = new BinaryFormatter();
            //XmlSerializer formatter = new XmlSerializer(typeof(TankInfo));
            //using (FileStream fs = new FileStream("tank.dat", FileMode.OpenOrCreate))
            //{
            //    TankInfo tank = (TankInfo)formatter.Deserialize(fs);

            //    Console.WriteLine("Объект десериализован");
            //    Console.WriteLine("Имя: {0} --- Возраст: {1}", tank.Name, tank.Year);
            //}

            //File.Delete("tank.dat");

            //Char delimiter = '|';

            //String[] substrings = builder.ToString().Split(delimiter);
            //foreach (var substring in substrings)
                //Console.WriteLine(substring);

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
