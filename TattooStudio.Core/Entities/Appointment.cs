using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int TattooRequestId { get; set; }
        public TattooRequest? TattooRequest { get; set; }

        [Display(Name = "Título do Evento")]
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Início do Agendamento")]
        [Required]
        public DateTime Start { get; set; }

        [Display(Name = "Fim do Agendamento")]
        [Required]
        public DateTime End { get; set; }
    }
}