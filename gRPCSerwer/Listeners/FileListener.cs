using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serwer.Attributes;
using Serwer.Communicators;

namespace Serwer.Listeners
{
    [Medium("FILE")]
    public class FileListener(params string[] paths) : IListener
    {
        private CommunicatorD? _onConnect;
        private List<FileCommunicator> _communicators;

        public void Start(CommunicatorD? onConnect)
        {
            _onConnect = onConnect;
            _communicators = [];
            foreach (var path in paths)
            {
                var communicator = new FileCommunicator(path);
                _communicators.Add(communicator);
                _onConnect?.Invoke(communicator);
            }
        }

        public void Stop()
        {
            foreach (var communicator in _communicators)
            {
                communicator.Stop();
            }
        }
    }
}
