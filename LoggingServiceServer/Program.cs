//
//
//
//

using LoggingServer.Services;
using LoggingServer.Configuration;





Console.WriteLine($"Current Working Directory: {Directory.GetCurrentDirectory()}");

ServerConfig config = ServerConfig.LoadConfig();
Server server = new Server(config);
await server.StartAsync();


