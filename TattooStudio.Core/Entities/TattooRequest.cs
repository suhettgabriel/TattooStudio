using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class TattooRequest
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Display(Name = "Estúdio")]
        [Required(ErrorMessage = "O estúdio é obrigatório.")]
        public int StudioId { get; set; }
        public Studio? Studio { get; set; }

        public string Status { get; set; } = "Nova Solicitação";

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public ICollection<TattooRequestAnswer> Answers { get; set; } = new List<TattooRequestAnswer>();

        public ICollection<Quote> Quotes { get; set; } = new List<Quote>();

        public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }
}