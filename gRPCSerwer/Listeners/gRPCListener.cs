using gRPCSerwer.Communicators;
using gRPCSerwer.gRPCServices;
using gRPCSerwer.Protos;
using Serwer;

namespace gRPCSerwer.Listeners
{
    public class gRPCListener : IListener
    {
        private CommunicatorD? _onConnect;
        private WebApplication app;

        public void Start(CommunicatorD? onConnect)
        {
            _onConnect = onConnect;
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddGrpc();
            app = builder.Build();
            app.MapGrpcService<gRPCCommunicator>();
            app.StartAsync();
        }

        public void Stop()
        {
            app.StopAsync();
        }
    }
}
