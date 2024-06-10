using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer
{
    public class Server
    {
        private static Server? _instance;

        private Dictionary<string, IServiceModule> services = new();
        private Dictionary<string, IListener> listeners = new();
        private List<ICommunicator> communicators = new();

        private Server()
        {
        }

        public static Server Instance
        {
            get
            {
                _instance ??= new Server();
                return _instance;
            }
        }

        public void AddServiceModule(string name, IServiceModule module)
        {
            services.Add(name, module);
        }

        public void AddCommunicator(ICommunicator communicator)
        {
            communicators.Add(communicator);
            communicator.Start(ServiceCenter, RemoveCommunicator);
        }

        public void AddListener(string name, IListener listener)
        {
            listeners.Add(name, listener);
        }

        public void RemoveServiceModule(string name)
        {
            services.Remove(name);
        }

        public void RemoveCommunicator(ICommunicator communicator)
        {
            communicators.Remove(communicator);
        }

        public void RemoveListener(string name)
        {
            listeners.Remove(name);
        }

        public void RemoveListener(IListener listener)
        {
            listeners.Remove(listeners.FirstOrDefault(x => x.Value == listener).Key);
        }

        public void Start()
        {
            foreach (var listener in listeners)
            {
                listener.Value.Start(AddCommunicator);
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
                return $"Service failure with exception: {e.Message}\n";
            }
        }

        private string GetCommandType(string command)
        {
            var spaceIndex = command.IndexOf(' ');
            return command[..spaceIndex];
        }
    }
}
