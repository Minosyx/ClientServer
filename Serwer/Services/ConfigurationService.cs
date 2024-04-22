using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Services
{
    public class ConfigurationService : IServiceModule
    {
        private readonly Dictionary<string, Func<string, string>> _actions;
        private readonly Server _server;

        public ConfigurationService(Server server)
        {
            _actions = new()
            {
                ["start-service"] = StartService,
                ["stop-service"] = StopService,
                ["start-medium"] = StartMedium,
                ["stop-medium"] = StopMedium,
            };

            _server = server;
        }

        private (string action, string data) ExtractParams(string command)
        {
            var commandPartIndex = command.IndexOf(' ');
            if (commandPartIndex == -1)
            {
                throw new ArgumentException("Invalid command");
            }
            var actionPartIndex = command.IndexOf(' ', commandPartIndex + 1);
            return actionPartIndex switch
            {
                -1 => (command[(commandPartIndex + 1)..], ""),
                _ => (command[(commandPartIndex + 1)..actionPartIndex], command[(actionPartIndex + 1)..])
            };
        }

        public string AnswerCommand(string command)
        {
            command = command.Trim();
            var (action, data) = ExtractParams(command);
            return _actions[action](data);
        }

        private string StartService(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return "Invalid command\n";
            }

            var parameters = data.Split(' ');
            var serviceName = parameters[0];
            var serviceType = parameters[1];
            var rest = parameters[2..];

            var serviceInstance = Activator.CreateInstance(Type.GetType(serviceType + "Service", false, true), rest);

            _server.AddServiceModule(serviceName, (IServiceModule) serviceInstance);
            return $"Service {serviceName} added successfully\n";
        }

        private string StopService(string data)
        {
            if (string.IsNullOrEmpty(data) || data.Contains(' '))
            {
                return "Invalid command\n";
            }

            _server.RemoveServiceModule(data);
            return $"Service {data} stopped\n";
        }

        private string StartMedium(string data)
        {
            var parameters = data.Split(' ');
            var mediumType = parameters[0];
            var rest = parameters[1..];

            var mediumInstance = Activator.CreateInstance(Type.GetType(mediumType + "Listener", false, true), rest);

            _server.AddListener((IListener) mediumInstance);
            return $"Medium {mediumType} started successfully\n";
        }

        private string StopMedium(string data)
        {
            if (string.IsNullOrEmpty(data) || data.Contains(' '))
            {
                return "Invalid command\n";
            }

            _server.RemoveListener((IListener) Activator.CreateInstance(Type.GetType(data + "Listener", false, true)));
            return $"Medium {data} stopped\n";
        }
    }
}
