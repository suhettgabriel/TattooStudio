using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class SystemSetting
    {
        public int Id { get; set; }

        [Display(Name = "Agenda Aberta para Novas Solicitações")]
        public bool IsAgendaOpen { get; set; } = true;

        [Display(Name = "Habilitar Termo de Consentimento no Portal")]
        public bool IsConsentFormEnabled { get; set; } = false;

        [Display(Name = "Texto do Termo de Consentimento")]
        public string? ConsentFormText { get; set; } 
    }
}