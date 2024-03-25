using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Klient.Communicators;
using Wspólne;

namespace Klient.Clients
{
    public class PingClient(ClientCommunicator communicator) : QAClient(communicator)
    {
        public double Test(int amount, int outputLen, int inputLen)
        {
            StringBuilder sb = new($"ping {inputLen} ");
            sb.Append(CommonTools.GenerateRandomAnswer(outputLen - sb.Length - 6));
            sb.Append('\n');
            string question = sb.ToString();
            string answer = "";

            DateTime now = DateTime.Now;
            for (int i = 0; i < amount; i++)
            {
                answer = communicator.QA(question);
            }
            TimeSpan diff = DateTime.Now - now;
            communicator.Close();

#if DEBUG
            Console.WriteLine($"Ping {inputLen} {outputLen} {amount} {diff.TotalMilliseconds / amount}");
            Console.WriteLine($"Answer: {answer}");
#endif

            return diff.TotalMilliseconds / amount;
        }
    }
}
