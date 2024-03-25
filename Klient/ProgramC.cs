using System.Runtime.CompilerServices;
using Klient.Clients;
using Klient.Communicators;


//TCPCommunicator communicator = new TCPCommunicator("localhost", 12345);

FileCommunicator communicator = new FileCommunicator(@"C:\Users\Patryk\Desktop\Asks");
PingClient pc = new PingClient(communicator);
double result = pc.Test(10, 1024, 4089);
Console.WriteLine($"Average time: {result}ms");