using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class FaqItem
    {
        public int Id { get; set; }

        [Display(Name = "Pergunta")]
        [Required(ErrorMessage = "A pergunta é obrigatória.")]
        public string Question { get; set; } = string.Empty;

        [Display(Name = "Resposta")]
        [Required(ErrorMessage = "A resposta é obrigatória.")]
        [DataType(DataType.MultilineText)]
        public string Answer { get; set; } = string.Empty;

        [Display(Name = "Ordem de Exibição")]
        public int DisplayOrder { get; set; }
    }
}