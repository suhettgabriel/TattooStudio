using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Entities
{
    public class SystemSetting
    {
        public int Id { get; set; }

        [Display(Name = "Agenda Aberta para Novas Solicitações")]
        public bool IsAgendaOpen { get; set; } = true;
    }
}