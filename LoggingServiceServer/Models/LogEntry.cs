﻿namespace LoggingServer.Models;

public class LogEntry
{
    public string Timestamp { get; set; } = "";
    public string Ip { get; set; } = "";
    public string Level { get; set; } = "";
    public string Message { get; set; } = "";

}
