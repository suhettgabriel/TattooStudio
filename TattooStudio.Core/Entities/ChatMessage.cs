using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required]
        public int TattooRequestId { get; set; }
        public TattooRequest? TattooRequest { get; set; }

        [Required]
        public string Sender { get; set; } = string.Empty; // "Admin" ou "Client"

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}