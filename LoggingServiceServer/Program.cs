using LoggingServer.Services;
using LoggingServer.Configuration;



Console.WriteLine("Hello World!");

Console.WriteLine($"Current Working Directory: {Directory.GetCurrentDirectory()}");

var config = ServerConfig.LoadConfig();
Console.WriteLine(config.ServerPort);
