using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.BLL.Contracts
{
    public interface IClientObject
    {
        void Process();
        string GetMessage();
        void Close();
    }
}
