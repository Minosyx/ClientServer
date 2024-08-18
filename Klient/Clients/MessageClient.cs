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
        private readonly ClientCommunicator _communicator = communicator;

        public bool Send(string[] recipients, string sender, string message)
        {
            StringBuilder sb = new("chat msg ");
            sb.Append(string.Join(';', recipients));
            sb.Append(' ');
            sb.Append(sender);
            sb.Append(' ');
            sb.Append(Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
            sb.Append('\n');
            string answer = _communicator.QA(sb.ToString());
            return !answer.Contains("error", StringComparison.CurrentCultureIgnoreCase);
        }

        public string Receive(string recipient)
        {
            string answer = _communicator.QA($"chat get {recipient}\n");
            var output = new StringBuilder();
            var messages = answer.Trim().Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var message in messages)
            {
                var content = message.Split(':', 2);
                output.Append($"{content[0].Trim()}: ");
                output.Append(Encoding.UTF8.GetString(Convert.FromBase64String(content[1].Trim())));
                output.Append('\n');
            }

            return output.ToString();
        }

        public string[] GetUsers()
        {
            string answer = _communicator.QA("chat who\n");
            return answer.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
