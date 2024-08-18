using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer
{
    public delegate string CommandD(string command);
    public delegate void CommunicatorD(ICommunicator commander);
    public delegate void MediumD(string name, IListener listener);
    public delegate void ServiceD(string name, IServiceModule module);
    public delegate void RemoveD(string name);

    public interface IServiceModule
    {
        string AnswerCommand(string command);
    }

    public interface ICommunicator
    {
        void Start(CommandD onCommand, CommunicatorD onDisconnect);
        void Stop();
    }

    public interface IListener
    {
        void Start(CommunicatorD? onConnect);
        void Stop();
    }
}
