using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klient.Communicators;

namespace Klient.Clients
{
    public class MessageClient(ClientCommunicator communicator) : QAClient(communicator)
    {
        public bool Send(string[] recipients, string sender, string message)
        {
            StringBuilder sb = new("chat msg ");
            sb.Append(string.Join(';', recipients));
            sb.Append(' ');
            sb.Append(sender);
            sb.Append(' ');
            sb.Append(message);
            sb.Append('\n');
            string answer = communicator.QA(sb.ToString());
            return !answer.Contains("error", StringComparison.CurrentCultureIgnoreCase);
        }

        public string Receive(string recipient)
        {
            string answer = communicator.QA($"chat get {recipient}\n");
            return answer.Trim();
        }

        public string[] GetUsers()
        {
            string answer = communicator.QA("chat who\n");
            return answer.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
