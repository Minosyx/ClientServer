using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer
{
    public class Server
    {
        private Dictionary<string, IServiceModule> services = new();
        private List<IListener> listeners = new(); // dictionary is needed
        private List<ICommunicator> communicators = new();

        public void AddServiceModule(string name, IServiceModule module)
        {
            services.Add(name, module);
        }

        public void AddCommunicator(ICommunicator communicator)
        {
            communicators.Add(communicator);
            communicator.Start(ServiceCenter, RemoveCommunicator);
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

        public void Start()
        {
            foreach (IListener listener in listeners)
            {
                listener.Start(AddCommunicator);
            }
        }

        public string ServiceCenter(string command)
        {
            try
            {
                var serviceName = GetCommandType(command);
                var service = services[serviceName];
                return service.AnswerCommand(command);
            }
            catch (Exception e)
            {
                return $"Service failure with exception: {e.Message}";
            }
        }

        private string GetCommandType(string command)
        {
            var spaceIndex = command.IndexOf(' ');
            return command[..spaceIndex];
        }
    }
}
