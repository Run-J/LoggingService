using LoggingServer.Models;

namespace LoggingServer.Services;

public class Logger
{
    // Properties
    private readonly string _logFilePath;


    // Methods
    public Logger(string logFilePath)
    {
        _logFilePath = logFilePath;
    }

    public async Task WriteLogAsync(LogEntry logEntry)
    {
        string jsonLog = JsonSerializer.Serialize(logEntry) + Environment.NewLine;
        await File.AppendAllTextAsync(_logFilePath, jsonLog);
    }
}