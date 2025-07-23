using System.ComponentModel.DataAnnotations;

namespace TattooStudio.Core.Enums
{
    public enum RequestStatus
    {
        [Display(Name = "Nova Solicitação")]
        NovaSolicitacao,

        [Display(Name = "Em Análise")]
        EmAnalise,

        [Display(Name = "Orçamento Enviado")]
        OrcamentoEnviado,

        [Display(Name = "Aguardando Sinal")]
        AguardandoSinal,

        [Display(Name = "Agendado")]
        Agendado,

        [Display(Name = "Lista de Espera")]
        ListaDeEspera,

        [Display(Name = "Recusado")]
        Recusado,

        [Display(Name = "Arquivado")]
        Arquivado
    }
}