using Klient.Communicators;

namespace Klient.Clients
{
    public abstract class QAClient
    {
        ClientCommunicator communicator;

        protected QAClient(ClientCommunicator communicator)
        {
            this.communicator = communicator;
        }
    }
}