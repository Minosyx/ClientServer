using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serwer.Attributes;

namespace Serwer.Services
{
    public class ConfigurationService : IServiceModule
    {
        private readonly Dictionary<string, Func<string, string>> _actions;

        private readonly MediumD _onMediumAdd;
        private readonly RemoveD _onMediumRemove;
        private readonly ServiceD _onServiceAdd;
        private readonly RemoveD _onServiceRemove;
        private readonly CommunicatorD _onConnect;

        public ConfigurationService(MediumD onMediumAdd, RemoveD onMediumRemove, ServiceD onServiceAdd,
            RemoveD onServiceRemove, CommunicatorD onConnect)
        {
            _onMediumAdd = onMediumAdd;
            _onMediumRemove = onMediumRemove;
            _onServiceAdd = onServiceAdd;
            _onServiceRemove = onServiceRemove;
            _onConnect = onConnect;

            _actions = new Dictionary<string, Func<string, string>>
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

        private Type? GetType<T>(string name) where T : Attribute, INamed
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(t => t.GetCustomAttributes<T>()
                    .Any(x => x.Name == name.ToUpper()));
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
            Type? serviceTypeInstance = GetType<ServiceAttribute>(serviceType);
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
            _onServiceAdd(serviceName, (IServiceModule)serviceInstance);
            return $"Service {serviceName} added successfully\n";
        }

        private string StopService(string data)
        {
            if (string.IsNullOrEmpty(data) || data.Contains(' '))
            {
                return "Invalid command\n";
            }

            _onServiceRemove(data);
            return $"Service {data} stopped\n";
        }

        private string StartMedium(string data)
        {
            var parameters = data.Split(' ');
            var mediumName = parameters[0];
            var mediumType = parameters[1];
            var rest = parameters[2..];

            Type? mediumTypeInstance = GetType<MediumAttribute>(mediumType);
            var mediumInstance = Activator.CreateInstance(mediumTypeInstance, args: rest);
            _onMediumAdd(mediumName, (IListener)mediumInstance);
            ((IListener) mediumInstance).Start(_onConnect);
            return $"Medium {mediumType} started successfully\n";
        }

        private string StopMedium(string data)
        {
            if (string.IsNullOrEmpty(data) || data.Contains(' '))
            {
                return "Invalid command\n";
            }

            _onMediumRemove(data);
            return $"Medium {data} stopped\n";
        }
    }
}
