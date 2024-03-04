using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer
{
    public class Server
    {
        private Dictionary<string, IServiceModule> services;
        private List<IListener> listeners;
        private List<ICommunicator> communicators;

        public void AddServiceModule(string name, IServiceModule module)
        {
            services.Add(name, module);
        }

        public void AddCommunicator(ICommunicator communicator)
        {
            communicators.Add(communicator);
        }

        public void AddListener(IListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveServiceModule(string name)
        {
            services.Remove(name);
        }

        public void RemoveCommunicator(ICommunicator communicator)
        {
            communicators.Remove(communicator);
        }

        public void RemoveListener(IListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
