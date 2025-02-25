namespace LoggingServer.Models;

public class LogEntry
{
    public string ClientTimestamp { get; set; } = "";
    public string ServerTimestamp { get; set; } = "";
    public string ClientIp { get; set; } = "";
    public string Level { get; set; } = "";
    public string Message { get; set; } = "";

}
