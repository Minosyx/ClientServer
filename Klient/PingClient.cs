using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Wspólne;

namespace Klient
{
    public class PingClient : QAClient
    {
        readonly ClientCommunicator _communicator;

        public PingClient(ClientCommunicator communicator) : base(communicator)
        {
            _communicator = communicator;
        }

        internal double Test(int amount, int outputLen, int inputLen)
        {
            StringBuilder sb = new($"ping {inputLen} ");
            sb.Append(CommandTools.GenerateRandomAnswer(outputLen - sb.Length - 6));
            sb.Append('\n');
            string question = sb.ToString();

            DateTime now = DateTime.Now;
            for (int i = 0; i < amount; i++)
            {
                string answer = _communicator.QA(question);

            }
            TimeSpan diff = DateTime.Now - now;
            return diff.TotalMilliseconds / amount;
        }
    }
}
