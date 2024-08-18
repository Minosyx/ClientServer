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

        private readonly Dictionary<string, IServiceModule> _services = [];
        private readonly Dictionary<string, IListener> _listeners = [];
        private readonly List<ICommunicator> _communicators = [];

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
            _services.TryAdd(name, module);
        }

        public void AddCommunicator(ICommunicator communicator)
        {
            _communicators.Add(communicator);
            communicator.Start(ServiceCenter, RemoveCommunicator);
        }

        public void AddListener(string name, IListener listener)
        {
            _listeners.TryAdd(name, listener);
        }

        public void RemoveServiceModule(string name)
        {
            _services.Remove(name);
        }

        public void RemoveCommunicator(ICommunicator communicator)
        {
            _communicators.Remove(communicator);
        }

        public void RemoveListener(string name)
        {
            _listeners.Remove(name);
        }

        public void RemoveListener(IListener listener)
        {
            _listeners.Remove(_listeners.FirstOrDefault(x => x.Value == listener).Key);
        }

        public void Start()
        {
            foreach (var listener in _listeners)
            {
                listener.Value.Start(AddCommunicator);
            }
        }

        public string ServiceCenter(string command)
        {
            try
            {
                var serviceName = GetCommandType(command);
                var service = _services[serviceName];
                return service.AnswerCommand(command);
            }
            catch (Exception e)
            {
                return $"Service failure with exception: {e.Message}\n";
            }
        }

        private static string GetCommandType(string command)
        {
            var spaceIndex = command.IndexOf(' ');
            return command[..spaceIndex];
        }
    }
}
