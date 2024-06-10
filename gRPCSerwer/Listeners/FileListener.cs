using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serwer.Communicators;

namespace Serwer.Listeners
{
    public class FileListener(params string[] paths) : IListener
    {
        private CommunicatorD? _onConnect;
        private List<FileCommunicator> _communicators;

        public void Start(CommunicatorD? onConnect)
        {
            _onConnect = onConnect;
            _communicators = new List<FileCommunicator>();
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
