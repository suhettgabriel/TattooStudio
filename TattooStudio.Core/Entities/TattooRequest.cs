using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Anotações da Análise")]
        public string? AnalysisNotes { get; set; }

        [Display(Name = "Tamanho Aproximado (cm)")]
        public int? EstimatedSize { get; set; }

        [Display(Name = "Estimativa Inicial (R$)")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? InitialEstimate { get; set; }

        [Display(Name = "Imagens de Referência")]
        public string? ReferenceImageUrls { get; set; }

        [Display(Name = "Foto do Local do Corpo")]
        public string? BodyPartPhotoUrl { get; set; }

        public string? MagicLinkToken { get; set; }
        public DateTime? MagicLinkTokenExpiration { get; set; }

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