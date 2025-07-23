using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public enum FormFieldType
    {
        [Display(Name = "Texto Curto")]
        TextoCurto,

        [Display(Name = "Texto Longo")]
        TextoLongo,

        [Display(Name = "Número")]
        Numero,

        [Display(Name = "Data")]
        Data,

        [Display(Name = "Lista de Opções (Dropdown)")]
        OpcaoUnica,

        [Display(Name = "Upload de Arquivo")]
        UploadArquivo
    }

    public class FormField
    {
        public int Id { get; set; }

        [Display(Name = "Texto da Pergunta (Label)")]
        [Required(ErrorMessage = "O texto da pergunta é obrigatório.")]
        [StringLength(255)]
        public string Label { get; set; } = string.Empty;

        [Display(Name = "Tipo de Campo")]
        [Required]
        public FormFieldType FieldType { get; set; }

        [Display(Name = "Opções (separadas por vírgula)")]
        public string? Options { get; set; }

        [Display(Name = "É obrigatório?")]
        [Required]
        public bool IsRequired { get; set; }

        [Display(Name = "Ordem de Exibição")]
        public int Order { get; set; }
    }
}