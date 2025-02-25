// File Name: LogEntry.cs
// Description: Represents a log entry that stores client and server timestamps, log level, and message details.
// Date: Feb 24, 2025

namespace LoggingServer.Models;


// Class Name: LogEntry
// Class Description:
//      - Defines the structure of a log entry.
//      - Stores timestamps, IP address, log level, and log message.
//      - Used for logging messages received from clients.
public class LogEntry
{
    public string ClientTimestamp { get; set; } = ""; // The timestamp recorded by the client when the log entry was generated.
    public string ServerTimestamp { get; set; } = ""; // The timestamp recorded by the server when the log entry was received.
    public string ClientIp { get; set; } = ""; // The IP address of the client that sent the log.
    public string Level { get; set; } = ""; // The severity level of the log entry.
    public string Message { get; set; } = ""; // The actual log message describing the event.

}
