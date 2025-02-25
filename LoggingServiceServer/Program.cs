// File Name: Program.cs
// Description: Entry point for the Logging Server application. Loads configuration and starts the server.
// Date: Feb 24, 2025


using LoggingServer.Services;
using LoggingServer.Configuration;


Console.WriteLine($"Current Working Directory: {Directory.GetCurrentDirectory()}");


// Load the server configuration from the JSON file (or use defaults if missing).
ServerConfig config = ServerConfig.LoadConfig();


// Create and start the logging server with the loaded configuration.
Server server = new Server(config);
await server.StartAsync();


