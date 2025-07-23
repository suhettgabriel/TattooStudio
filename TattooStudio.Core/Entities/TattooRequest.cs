using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class TattooRequest
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        public string Status { get; set; } = "Nova Solicitação";

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public ICollection<TattooRequestAnswer> Answers { get; set; } = new List<TattooRequestAnswer>();
    }
}