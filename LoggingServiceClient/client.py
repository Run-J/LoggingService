import socket
import json
import time
import os


def load_config(config_path="clientconfig.json"):
    """
    Loads the client configuration from a JSON file.
    If the file is missing or invalid, returns default values.
    """
    if not os.path.exists(config_path):
        print("[Client] 'clientconfig.json' not found; using defaults (127.0.0.1, 13000).")
        return {"serverIp": "127.0.0.1", "serverPort": 13000}

    try:
        with open(config_path, "r") as f:
            data = json.load(f)
            server_ip = data.get("serverIp", "127.0.0.1")
            server_port = data.get("serverPort", 13000)
            return {"serverIp": server_ip, "serverPort": server_port}
    except Exception as ex:
        print(f"[Client] Failed to parse config file: {ex}. Using defaults.")
        return {"serverIp": "127.0.0.1", "serverPort": 13000}


def send_log(server_ip, server_port, log_entry):
    """
    Sends a single log entry to the server via TCP.
    Ensures the JSON is terminated by a newline to match the server's line-based read.
    """
    try:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((server_ip, server_port))

            # Convert the log_entry dictionary to JSON + newline
            json_log = json.dumps(log_entry) + "\n"
            s.sendall(json_log.encode("utf-8"))

            print(f"[Client] Log sent successfully: {log_entry['Message']}")
    except Exception as ex:
        print(f"[Client] Error sending log: {ex}")


def manual_logging(server_ip, server_port):
    """
    Prompt the user for a log level and message, then send one log entry.
    """
    level = input("Enter log level (Debug, Info, Warning, Error, Fatal): ").strip() or "Info"
    message = input("Enter log message: ").strip() or "Test log message"

    log_entry = {
        "ClientTimestamp": time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime()),  # UTC in ISO8601
        "Level": level,
        "Message": message
    }

    send_log(server_ip, server_port, log_entry)


def noisy_logging(server_ip, server_port):
    """
    Send multiple log entries in quick succession to test the server's rate-limiting.
    """
    try:
        count = int(input("How many logs do you want to send? (e.g. 5): "))
        delay_ms = int(input("Enter the delay in milliseconds between logs (e.g. 500): "))
    except ValueError:
        print("[Client] Invalid input, defaulting to 5 logs, 1000 ms delay.")
        count = 5
        delay_ms = 1000

    for i in range(count):
        log_entry = {
            "ClientTimestamp": time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime()),
            "Level": "Info",  # or let user pick each time, if you prefer
            "Message": f"Noisy log #{i + 1}"
        }
        send_log(server_ip, server_port, log_entry)
        time.sleep(delay_ms / 1000.0)  # Convert ms to seconds


def main_menu(server_ip, server_port):
    """
    Display the main menu, handle user's choices, and loop until exit.
    """
    while True:
        print("\n========== PYTHON LOG CLIENT ==========")
        print("[1] Manual Log Entry")
        print("[2] Noisy Logs (Test Rate Limiting)")
        print("[3] Exit")
        choice = input("Choose an option: ").strip()

        if choice == "1":
            manual_logging(server_ip, server_port)
        elif choice == "2":
            noisy_logging(server_ip, server_port)
        elif choice == "3":
            print("[Client] Exiting client.")
            break
        else:
            print("[Client] Invalid choice. Please try again.")


if __name__ == "__main__":
    config = load_config()  # Load from 'clientconfig.json' by default
    ip = config["serverIp"]
    port = config["serverPort"]

    print(f"[Client] Connecting to server at {ip}:{port}...")
    main_menu(ip, port)
