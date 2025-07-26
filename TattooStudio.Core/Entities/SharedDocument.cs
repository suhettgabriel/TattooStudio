using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class SharedDocument
    {
        public int Id { get; set; }

        [Display(Name = "Nome do Arquivo")]
        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FileUrl { get; set; } = string.Empty;

        [Display(Name = "Descrição (Opcional)")]
        public string? Description { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}