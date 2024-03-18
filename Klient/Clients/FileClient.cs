using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klient.Communicators;

namespace Klient.Clients
{
    public class FileClient(ClientCommunicator communicator) : QAClient(communicator)
    {
        private readonly ClientCommunicator _communicator = communicator;
    }
}
