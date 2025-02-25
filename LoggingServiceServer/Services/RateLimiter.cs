// File Name: RateLimiter.cs
// Description: Implements rate limiting to prevent clients from logging too frequently.
// Date: Feb 24, 2025


namespace LoggingServer.Services;



// Class Name: RateLimiter
// Class Description: 
//      - Maintains a dictionary of client IPs and their last log time.
//      - Enforces a time-based restriction to prevent excessive logging.
//      - Helps protect the server from misconfigured or abusive clients.
public class RateLimiter
{
    // Attributes
    private readonly int _limitSeconds; // The minimum interval (in seconds) that a client must wait before logging again.
    private readonly ConcurrentDictionary<string, DateTime> _clients; // A thread-safe dictionary that stores the last log timestamp for each client IP.


    // Methods

    // Method Name: RateLimiter (Constructor)
    // Parameters: 
    //      - int limitSeconds: The time interval (in seconds) a client must wait before logging again.
    // Return Value: None
    // Description: Initializes the rate limiter with a specified time limit.
    public RateLimiter(int limitSeconds)
    {
        _limitSeconds = limitSeconds;
        _clients = new ConcurrentDictionary<string, DateTime>();
    }



    // Method Name: CanLog
    // Parameters: 
    //      - string clientIp: The IP address of the client attempting to log.
    // Return Value: 
    //      - bool: Returns `true` if the client is allowed to log, `false` if they are rate-limited.
    // Description:
    //      - Checks if the given client IP has logged within the last `_limitSeconds` seconds.
    //      - If the client has exceeded the rate limit, it returns `false`.
    //      - Otherwise, it updates the client's last log time and returns `true`.
    public bool CanLog(string clientIp)
    {
        var lastLogTime = _clients.GetOrAdd(clientIp, DateTime.MinValue);
        if ((DateTime.UtcNow - lastLogTime).TotalSeconds < _limitSeconds)
            return false;

        _clients[clientIp] = DateTime.UtcNow;
        return true;
    }
}