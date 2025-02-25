// File Name: ServerConfig.cs
// Description: Manages server configuration settings, including port, log file path, and rate limit settings.
// Date: Feb 24, 2025
namespace LoggingServer.Configuration;



// Class Name: ServerConfig
// Class Description:
//      - Stores configuration settings for the logging server.
//      - Loads settings from a JSON configuration file (`appsettings.json`).
//      - Provides default values if the configuration file is missing.
public class ServerConfig
{
    // Properties
    public int ServerPort { get; set; }   // Specifies the port number the server listens on.
    public string LogFilePath { get; set; } = "";  // Specifies the path where log entries are stored.
    public int RateLimitSeconds { get; set; }  // Specifies the rate limit duration (in seconds) for log entries from a single client.



    // Methods

    // Method Name: LoadConfig
    // Parameters:
    //      - string logFilePath (optional): The path to the JSON configuration file. Defaults to "appsettings.json".
    // Return Value:
    //      - ServerConfig: An instance of ServerConfig containing the loaded settings.
    // Description:
    //      - Loads the configuration settings from the specified JSON file.
    //      - If the file is missing or invalid, it returns default settings.
    public static ServerConfig LoadConfig(string logFilePath = "appsettings.json")
    {
        if (!File.Exists(logFilePath))
        {
            Console.WriteLine("[ServerConfig] Configuration file missing, using defaults.");
            return new ServerConfig
            {
                ServerPort = 5000,
                LogFilePath = "logs.json",
                RateLimitSeconds = 5
            };
        }


        string json = File.ReadAllText(logFilePath);
        return JsonSerializer.Deserialize<ServerConfig>(json) ?? new ServerConfig();
    }
}
