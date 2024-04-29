using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klient.Communicators;

namespace Klient.Clients
{
    public class ConfigurationClient(ClientCommunicator communicator) : QAClient(communicator)
    {
        private void Close()
        {
            communicator.Close();
        }

        public string StartService(string serviceName, string serviceType, params string[]? args)
        {
            string answer = communicator.QA($"config start-service {serviceName} {serviceType} {string.Join(" ", args)}\n");
            Close();
            return answer.Trim();
        }

        public string StopService(string serviceName)
        {
            string answer = communicator.QA($"config stop-service {serviceName}\n");
            Close();
            return answer.Trim();
        }

        public string StartMedium(string mediumName, string mediumType, params string[]? args)
        {
            string answer = communicator.QA($"config start-medium {mediumName} {mediumType} {string.Join(" ", args)}\n");
            Close();
            return answer.Trim();
        }

        public string StopMedium(string mediumName)
        {
            string answer = communicator.QA($"config stop-medium {mediumName}\n");
            Close();
            return answer.Trim();
        }
    }
}
