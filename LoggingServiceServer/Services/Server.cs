// File Name: Server.cs
// Description: Implements a TCP logging server that receives log messages from clients, applies rate limiting, and writes logs to a file.
// Date: Feb 24, 2025


using LoggingServer.Configuration;
using LoggingServer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoggingServer.Services;


// Class Name: Server
// Class Description:
//      - Starts a TCP server that listens for incoming log messages from clients.
//      - Handles multiple clients asynchronously.
//      - Applies rate limiting to prevent excessive logging.
//      - Converts UTC timestamps to Toronto local time before storing logs.
public class Server
{
    private readonly int _port; // The port number on which the server listens for incoming connections.
    private readonly Logger _logger; // Handles writing received log entries to a file.
    private readonly RateLimiter _rateLimiter; // Manages rate limiting to prevent abuse by misconfigured or overly chatty clients.


    // Method Name: Server (Constructor)
    // Parameters:
    //      - ServerConfig config: The configuration object containing server settings.
    // Return Value: None
    // Description:
    //      - Initializes the server with a specified port, log file path, and rate limiting settings.
    public Server(ServerConfig config)
    {
        _port = config.ServerPort;
        _logger = new Logger(config.LogFilePath);
        _rateLimiter = new RateLimiter(config.RateLimitSeconds);
    }


    // Method Name: StartAsync
    // Parameters: None
    // Return Value:
    //      - Task: Asynchronous operation with no return value.
    // Description:
    //      - Starts the TCP server and listens for incoming client connections.
    //      - Accepts and processes multiple client connections asynchronously.
    public async Task StartAsync()
    {
        TcpListener listner = new TcpListener(IPAddress.Any, _port);
        listner.Start();
        Console.WriteLine($"[Server] Listening on port {_port}... ");

        while (true)
        {
            TcpClient client = await listner.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }



    // Method Name: HandleClientAsync
    // Parameters:
    //      - TcpClient client: The connected client.
    // Return Value:
    //      - Task: Asynchronous operation with no return value.
    // Description:
    //      - Handles communication with a single client.
    //      - Reads incoming log messages from the client.
    //      - Applies rate limiting to prevent excessive logging.
    //      - Converts UTC timestamps to Toronto local time before saving logs.
    //      - Writes valid logs to the log file.
    private async Task HandleClientAsync(TcpClient client)
    {
        string clientIp = ((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString();
        Console.WriteLine($"[Server] Client connected: {clientIp}");

        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream, Encoding.UTF8);


        try
        {
            string? message = await reader.ReadLineAsync();
            if (message == null)
            {
                return;
            }


            if (!_rateLimiter.CanLog(clientIp))
            {
                Console.WriteLine($"[Server] Rate limit triggered for {clientIp}");
                return;
            }


            var logEntry = JsonSerializer.Deserialize<LogEntry>(message);
            if (logEntry is not null)
            {
                TimeZoneInfo torontoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Toronto");
                DateTime torontoTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, torontoTimeZone);

                logEntry.ServerTimestamp = torontoTime.ToString("o");
                logEntry.ClientIp = clientIp;

                await _logger.WriteLogAsync(logEntry);
                Console.WriteLine($"[Server] Logged from {clientIp}: {logEntry.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Server] Error: {ex.Message}");
        }
        finally
        {
            client.Close();
            Console.WriteLine($"[Server] Client disconnected: {clientIp}");
        }
    }
}
