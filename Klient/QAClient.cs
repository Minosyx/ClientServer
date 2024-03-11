namespace Klient
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