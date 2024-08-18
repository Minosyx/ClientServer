using Klient.Communicators;

namespace Klient.Clients
{
    public abstract class QAClient(ClientCommunicator communicator)
    {
        private readonly ClientCommunicator communicator = communicator;
    }
}