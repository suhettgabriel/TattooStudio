using System.ComponentModel.DataAnnotations;
using TattooStudio.Core.Enums;

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

        public RequestStatus Status { get; set; } = RequestStatus.NovaSolicitacao;

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public ICollection<TattooRequestAnswer> Answers { get; set; } = new List<TattooRequestAnswer>();

        public ICollection<Quote> Quotes { get; set; } = new List<Quote>();

        public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

        public Appointment? Appointment { get; set; }

        public string GetAnswerByLabel(string label)
        {
            var answer = Answers.FirstOrDefault(a => a.FormField?.Label.Equals(label, StringComparison.OrdinalIgnoreCase) ?? false);
            return answer?.Value ?? "Cliente";
        }
    }
}