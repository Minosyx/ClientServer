using Serwer;
using Serwer.Listeners;


Server server = new Server();
server.AddServiceModule("ping",new PingService());
//server.AddListener(new TCPListener(12345));
server.AddListener(new FileListener(@"C:\Users\Patryk\Desktop\Asks"));
server.Start();