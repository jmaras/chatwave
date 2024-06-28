using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ChatWaveServerApp.Components.Hubs
{
    public class PrivateChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> userConnections = new();

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                userConnections[userName] = Context.ConnectionId;
                Console.WriteLine($"User connected: {userName} with connection ID: {Context.ConnectionId}");
            }
            else
            {
                Console.WriteLine("User is not authenticated.");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userName = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                userConnections.TryRemove(userName, out _);
                Console.WriteLine($"User disconnected: {userName}");
            }
            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendPrivateMessage(string senderUserName, string recipientUserName, string message)
        {
            DateTime dateTime = DateTime.Now;
            Console.WriteLine($"Sending private message from {senderUserName} to {recipientUserName}: {message} at {dateTime}");

            if (userConnections.TryGetValue(recipientUserName, out var connectionId))
            {
                Console.WriteLine($"Recipient {recipientUserName} is connected with connection ID: {connectionId}");
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", senderUserName, recipientUserName, message, dateTime);
            }
            else
            {
                Console.WriteLine($"Recipient {recipientUserName} not found");
            }
        }
    }
}
