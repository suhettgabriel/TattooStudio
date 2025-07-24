using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class GalleryImage
    {
        public int Id { get; set; }

        [Display(Name = "URL da Imagem")]
        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Descrição (Legenda)")]
        [StringLength(255)]
        public string? Description { get; set; }

        [Display(Name = "Ordem de Exibição")]
        public int DisplayOrder { get; set; }
    }
}