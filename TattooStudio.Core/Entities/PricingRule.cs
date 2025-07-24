using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TattooStudio.Core.Entities
{
    public class PricingRule
    {
        public int Id { get; set; }

        [Display(Name = "Parte do Corpo")]
        [Required(ErrorMessage = "A parte do corpo é obrigatória.")]
        public string BodyPart { get; set; } = string.Empty;

        [Display(Name = "Tamanho Mínimo (cm)")]
        [Required(ErrorMessage = "O tamanho mínimo é obrigatório.")]
        public int MinSize { get; set; }

        [Display(Name = "Tamanho Máximo (cm)")]
        [Required(ErrorMessage = "O tamanho máximo é obrigatório.")]
        public int MaxSize { get; set; }

        [Display(Name = "Preço Base (R$)")]
        [Required(ErrorMessage = "O preço base é obrigatório.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal BasePrice { get; set; }
    }
}