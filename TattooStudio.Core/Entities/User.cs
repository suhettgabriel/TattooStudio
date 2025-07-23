using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O WhatsApp é obrigatório.")]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string? InstagramHandle { get; set; }

        public ICollection<TattooRequest> TattooRequests { get; set; } = new List<TattooRequest>();
    }
}