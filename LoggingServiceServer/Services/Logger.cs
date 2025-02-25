// File Name: Logger.cs
// Description: Handles writing log entries to a JSON file asynchronously.
// Date: Feb 24, 2025


using LoggingServer.Models;

namespace LoggingServer.Services;


// Class Name: Logger
// Class Description:
//      - Manages log storage by writing log entries to a specified file.
//      - Ensures logs are stored in JSON format with each entry on a new line.
//      - Uses asynchronous operations to prevent blocking the server.
public class Logger
{
    // Properties
    private readonly string _logFilePath; // Stores the file path where log entries are written.


    // Methods

    // Method Name: Logger (Constructor)
    // Parameters:
    //      - string logFilePath: The path to the log file.
    // Return Value: None
    // Description:
    //      - Initializes the Logger with a specified log file path.
    public Logger(string logFilePath)
    {
        _logFilePath = logFilePath;
    }


    // Method Name: WriteLogAsync
    // Parameters:
    //      - LogEntry logEntry: The log entry to be written.
    // Return Value:
    //      - Task: Asynchronous operation with no return value.
    // Description:
    //      - Serializes the log entry to JSON format.
    //      - Appends the serialized JSON entry to the log file asynchronousl
    public async Task WriteLogAsync(LogEntry logEntry)
    {
        string jsonLog = JsonSerializer.Serialize(logEntry) + Environment.NewLine;
        await File.AppendAllTextAsync(_logFilePath, jsonLog);
    }
}