using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serwer.Communicators;

namespace Serwer.Listeners
{
    public class RS232Listener(params string[] ports) : IListener
    {
        private CommunicatorD? _onConnect;
        private List<RS232Communicator> _communicators;

        public void Start(CommunicatorD? onConnect)
        {
            _onConnect = onConnect;
            _communicators = new List<RS232Communicator>();
            foreach (var port in ports)
            {
                var communicator = new RS232Communicator(port);
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
