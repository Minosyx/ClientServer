using Serwer;
using Serwer.Listeners;


Server server = new Server();
server.AddServiceModule("ping",new PingService());
server.AddListener(new TCPListener(12345));
server.Start();