using Serwer;


Console.WriteLine(new PingService().AnswerCommand("ping 100 abc"));
Server server = new Server();
server.AddServiceModule("ping",new PingService());
server.AddListener(new TCPListener());
server.Start();