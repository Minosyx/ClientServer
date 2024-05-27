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

        public ConfigurationService()
        {
            _actions = new()
            {
                ["start-service"] = StartService,
                ["stop-service"] = StopService,
                ["start-medium"] = StartMedium,
                ["stop-medium"] = StopMedium,
            };
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

            object? serviceInstance;
            Type? serviceTypeInstance = Type.GetType($"serwer.services.{serviceType}service", false, true);
            try
            {
                serviceInstance = rest.Length == 0
                    ? Activator.CreateInstance(serviceTypeInstance)
                    : Activator.CreateInstance(serviceTypeInstance, args: rest);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

            Server.Instance.AddServiceModule(serviceName, (IServiceModule) serviceInstance);
            return $"Service {serviceName} added successfully\n";
        }

        private string StopService(string data)
        {
            if (string.IsNullOrEmpty(data) || data.Contains(' '))
            {
                return "Invalid command\n";
            }

            Server.Instance.RemoveServiceModule(data);
            return $"Service {data} stopped\n";
        }

        private string StartMedium(string data)
        {
            var parameters = data.Split(' ');
            var mediumName = parameters[0];
            var mediumType = parameters[1];
            var rest = parameters[2..];

            object? mediumInstance;
            Type? mediumTypeInstance = Type.GetType($"serwer.listeners.{mediumType}Listener", false, true);
            mediumInstance = Activator.CreateInstance(mediumTypeInstance, args: rest);
            Server.Instance.AddListener(mediumName, (IListener) mediumInstance);
            ((IListener) mediumInstance).Start(Server.Instance.AddCommunicator);
            return $"Medium {mediumType} started successfully\n";
        }

        private string StopMedium(string data)
        {
            if (string.IsNullOrEmpty(data) || data.Contains(' '))
            {
                return "Invalid command\n";
            }

            Server.Instance.RemoveListener(data);

            return $"Medium {data} stopped\n";
        }
    }
}
