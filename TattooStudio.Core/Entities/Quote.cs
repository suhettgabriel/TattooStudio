using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TattooStudio.Core.Entities
{
    public enum QuoteStatus
    {
        [Display(Name = "Pendente")]
        Pendente,
        [Display(Name = "Aprovado")]
        Aprovado,
        [Display(Name = "Recusado")]
        Recusado
    }

    public class Quote
    {
        public int Id { get; set; }

        [Required]
        public int TattooRequestId { get; set; }
        public TattooRequest? TattooRequest { get; set; }

        [Display(Name = "Valor Total (R$)")]
        [Required(ErrorMessage = "O valor total é obrigatório.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Display(Name = "Valor do Sinal (R$)")]
        [Required(ErrorMessage = "O valor do sinal é obrigatório.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DepositAmount { get; set; }

        [Display(Name = "Descrição / Detalhes")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Válido Até")]
        [Required(ErrorMessage = "A data de validade é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Status")]
        public QuoteStatus Status { get; set; } = QuoteStatus.Pendente;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}