using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class Studio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da cidade é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da cidade não pode ter mais de 100 caracteres.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do estado é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do estado não pode ter mais de 100 caracteres.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(255, ErrorMessage = "O endereço não pode ter mais de 255 caracteres.")]
        public string Address { get; set; } = string.Empty;
    }
}