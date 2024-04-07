using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Services
{
    public class MessageService : IServiceModule
    {
        private Dictionary<string, Dictionary<string, string>> _messages;

        private readonly Dictionary<string, Func<string, string>> _actions;

        public MessageService()
        {
            _messages = [];
            _actions = new()
            {
                ["msg"] = SendMessage,
                ["get"] = GetMessage,
            };
        }

        public string AnswerCommand(string command)
        {
            command = command.Trim();
            var (action, data) = ExtractParams(command);
            return _actions[action](data);
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
                -1 when command[(commandPartIndex + 1)..] != "who" => throw new ArgumentException("Invalid command"),
                _ => (command[(commandPartIndex + 1)..actionPartIndex], command[(actionPartIndex + 1)..])
            };
        }

        private string SendMessage(string data)
        {
            int recipientsEndIndex = data.IndexOf(' ');
            if (recipientsEndIndex == -1)
            {
                return "Invalid command\n";
            }
            int senderEndIndex = data.IndexOf(' ', recipientsEndIndex + 1);
            if (senderEndIndex == -1)
            {
                return "Invalid command\n";
            }

            string[] recipients = data[..recipientsEndIndex].Split(';');
            string sender = data[(recipientsEndIndex + 1)..senderEndIndex];
            string message = data[(senderEndIndex + 1)..];

            foreach (var recipient in recipients)
            {
                if (!_messages.TryGetValue(recipient, out Dictionary<string, string>? value))
                {
                    value = [];
                    _messages[recipient] = value;
                }

                value.Add(sender, message);
            }
            return "Message sent\n";
        }

        private string GetMessage(string data)
        {
            data = data.Trim();
            if (!_messages.TryGetValue(data, out Dictionary<string, string>? messages) || messages.Count == 0)
            {
                return "No messages\n";
            }
            else
            {
                StringBuilder sb = new();
                foreach (var (sender, message) in messages)
                {
                    sb.Append(sender);
                    sb.Append(": ");
                    sb.Append(message);
                    sb.Append('\n');
                }
                return sb.ToString();
            }
        }
    }
}
