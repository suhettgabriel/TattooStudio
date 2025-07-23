using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class TattooRequestAnswer
    {
        public int Id { get; set; }

        [Required]
        public int TattooRequestId { get; set; }
        public TattooRequest? TattooRequest { get; set; }

        [Required]
        public int FormFieldId { get; set; }
        public FormField? FormField { get; set; }

        [Required(ErrorMessage = "A resposta não pode ser vazia.")]
        public string Value { get; set; } = string.Empty;
    }
}