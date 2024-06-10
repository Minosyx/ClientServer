using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Klient.Clients;

namespace Klient.Communicators
{
    public class gRPCCommunicator : ClientCommunicator
    {
        private GrpcChannel _channel;
        private Messenger.MessengerClient _client;

        public gRPCCommunicator(string address)
        {
            _channel = GrpcChannel.ForAddress(address);
            _client = new Messenger.MessengerClient(_channel);
        }

        public override string QA(string question)
        {
            return _client.SendMessage(new MessageRequest {Message = question}).Message;
        }

        public override void Close()
        {
            _channel.Dispose();
        }
    }
}
