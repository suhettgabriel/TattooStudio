using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public enum EmailTemplateType
    {
        [Display(Name = "Confirmação de Solicitação")]
        RequestConfirmation,

        [Display(Name = "Orçamento Disponível")]
        QuoteAvailable,

        [Display(Name = "Agendamento Confirmado")]
        BookingConfirmed,

        [Display(Name = "Lembrete de Agendamento")]
        BookingReminder
    }

    public class EmailTemplate
    {
        public int Id { get; set; }

        [Required]
        public EmailTemplateType Type { get; set; }

        [Display(Name = "Assunto do E-mail")]
        [Required(ErrorMessage = "O assunto é obrigatório.")]
        public string Subject { get; set; } = string.Empty;

        [Display(Name = "Corpo do E-mail")]
        [Required(ErrorMessage = "O corpo do e-mail é obrigatório.")]
        public string Body { get; set; } = string.Empty;

        [Display(Name = "Variáveis Disponíveis")]
        public string Placeholders { get; set; } = string.Empty;
    }
}