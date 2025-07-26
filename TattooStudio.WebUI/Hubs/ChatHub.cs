using Microsoft.AspNetCore.SignalR;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(int tattooRequestId, string sender, string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            var chatMessage = new ChatMessage
            {
                TattooRequestId = tattooRequestId,
                Sender = sender,
                Message = message,
                IsRead = false,
                SentAt = DateTime.Now
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            var sentAtFormatted = chatMessage.SentAt.ToString("dd/MM/yy HH:mm");

            await Clients.Group(tattooRequestId.ToString()).SendAsync("ReceiveMessage", sender, message, sentAtFormatted);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}