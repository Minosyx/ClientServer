using Klient;


TCPCommunicator communicator = new TCPCommunicator("localhost", 12345);
PingClient pc = new PingClient(communicator);
double result = pc.Test(10, 1024, 2048);
Console.WriteLine($"Average time: {result}ms");