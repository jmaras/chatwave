using Microsoft.AspNetCore.SignalR;

namespace ChatWaveServerApp.Components.Hubs
{
    /// <summary>
    /// Represents a hub for real-time chat functionality.
    /// </summary>
    public class PublicChatHub : Hub
    {
        /// <summary>
        /// Sends a chat message to all connected clients.
        /// </summary>
        /// <param name="userName">The name of the user sending the message.</param>
        /// <param name="message">The content of the message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendMessage(string userName, string message)
        {
            DateTime dateTime = DateTime.Now;
            await Clients.All.SendAsync("ReceiveMessage", userName, message, dateTime);
        }
    }
}
