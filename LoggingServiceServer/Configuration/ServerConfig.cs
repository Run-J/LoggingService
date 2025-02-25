namespace LoggingServer.Configuration;

public class ServerConfig
{
    // Properties
    public int ServerPort { get; set; }
    public string LogFilePath { get; set; } = "";


    // Methods
    public static ServerConfig LoadConfig(string logFilePath = "appsettings.json")
    {
        if (!File.Exists(logFilePath))
        {
            Console.WriteLine("[ServerConfig] Configuration file missing, using defaults.");
            return new ServerConfig
            {
                ServerPort = 5000,
                LogFilePath = "logs.json"
            };
        }


        string json = File.ReadAllText(logFilePath);
        return JsonSerializer.Deserialize<ServerConfig>(json) ?? new ServerConfig();
    }
}
