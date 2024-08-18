using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serwer.Attributes;

namespace Serwer.Services
{
    [Service("FTP")]
    public class FileService : IServiceModule
    {
        private readonly string _rootDir;

        private readonly Dictionary<string, Func<string, string>> _actions;

        public FileService(string rootDir)
        {
            _rootDir = rootDir;

            _actions = new Dictionary<string, Func<string, string>>
            {
                ["put"] = PutFile,
                ["get"] = GetFile,
                ["dir"] = GetDir
            };
        }

        public string AnswerCommand(string command)
        {
            command = command.Trim();
            var (action, data) = ExtractParams(command);
            return _actions[action](data);
        }

        private static (string action, string data) ExtractParams(string command)
        {
            var commandPartIndex = command.IndexOf(' ');
            if (commandPartIndex == -1)
            {
                throw new ArgumentException("Invalid command");
            }
            var actionPartIndex = command.IndexOf(' ', commandPartIndex + 1);
            return actionPartIndex switch
            {
                -1 when command[(commandPartIndex + 1)..] != "dir" => throw new ArgumentException("Invalid command"),
                -1 => (command[(commandPartIndex + 1)..], ""),
                _ => (command[(commandPartIndex + 1)..actionPartIndex], command[(actionPartIndex + 1)..])
            };
        }

        private string PutFile(string data)
        {
            var i = data.IndexOf(' ');
            var fileName = data[..i];
            data = data[(i + 1)..];
            var fileContent = Convert.FromBase64String(data);
            var path = $@"{_rootDir}\{fileName}";
            File.WriteAllBytes(path, fileContent);
            return "File saved\n";
        }

        private string GetFile(string data)
        {
            var path = $@"{_rootDir}\{data}";
            if (!File.Exists(path))
            {
                return "File not found\n";
            }
            var b64 = Convert.ToBase64String(File.ReadAllBytes(path));
            return $"{b64}\n";
        }

        private string GetDir(string data)
        {
            var files = Directory.GetFiles($@"{_rootDir}\{data}");
            return string.Join('\n', files.Select(f => f[(_rootDir.Length + 1)..])) + '\n';
        }
    }
}
