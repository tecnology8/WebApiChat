using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiChat.Models
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string name, string text)
        {
            var message = new ChatMessage
            {
                SenderName = name,
                Text = text,
                SendAt = DateTime.Now
            };
            // invoke this ReceiveMessage method in the client
            // Broadcast to all clients
            await Clients.All.SendAsync(
                    "ReceiveMessage",
                    message.SenderName,
                    message.Text,
                    message.SendAt
                );
        }
    }
}
