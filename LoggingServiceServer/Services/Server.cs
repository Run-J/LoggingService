using LoggingServer.Configuration;
using LoggingServer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoggingServer.Services;

public class Server
{
    private readonly int _port;
    private readonly Logger _logger;
    
    public Server(ServerConfig config)
    {
        _port = config.ServerPort;
        _logger = new Logger(config.LogFilePath);
    }


    public async Task StartAsync()
    {
        TcpListener listner = new TcpListener(IPAddress.Any, _port);
        listner.Start();
        Console.WriteLine($"[Server] Listening on port {_port}... ");

        while (true)
        {
            TcpClient client = await listner.AcceptTcpClientAsync();
            Console.WriteLine($"[Server] Current Thread: {Thread.CurrentThread.ManagedThreadId}");

            _ = HandleClientAsync(client);
        }
    }


    private async Task HandleClientAsync(TcpClient client)
    {
        Console.WriteLine($"[Server] Current Thread: {Thread.CurrentThread.ManagedThreadId}");

        string clientIp = ((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString();
        Console.WriteLine($"[Server] Client connected: {clientIp}");

        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream, Encoding.UTF8);


        try
        {
            //// Buffer to store the response bytes.
            //Byte[] data = new Byte[256];

            //// String to store the response ASCII representation.
            //System.String responseData = System.String.Empty;

            //// Read the first batch of the TcpServer response bytes.
            //Int32 bytes = stream.Read(data, 0, data.Length);
            //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            //Console.WriteLine("Received: {0}", responseData);

            string? message = await reader.ReadLineAsync();
            if (message == null)
            {
                return;
            }


            var logEntry = JsonSerializer.Deserialize<LogEntry>(message);
            if (logEntry is not null)
            {
                logEntry.Ip = clientIp;
                logEntry.Timestamp = DateTime.UtcNow.ToString("o");

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
