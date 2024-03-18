namespace Klient.Communicators
{
    public abstract class ClientCommunicator
    {
        public abstract string QA(string question);
        public abstract void Close();
    }
}