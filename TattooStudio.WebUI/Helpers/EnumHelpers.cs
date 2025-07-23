using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using TattooStudio.Core.Enums;

namespace TattooStudio.WebUI.Helpers
{
    public static class EnumHelpers
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName() ?? enumValue.ToString();
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj) where TEnum : struct, Enum
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.GetDisplayName() };
            return new SelectList(values, "Id", "Name", enumObj);
        }

        public static string GetStatusColor(RequestStatus status)
        {
            return status switch
            {
                RequestStatus.NovaSolicitacao => "#0d6efd",
                RequestStatus.EmAnalise => "#6f42c1",
                RequestStatus.OrcamentoEnviado => "#0dcaf0",
                RequestStatus.AguardandoSinal => "#ffc107",
                RequestStatus.Agendado => "#198754",
                RequestStatus.ListaDeEspera => "#fd7e14",
                RequestStatus.Recusado => "#dc3545",
                RequestStatus.Arquivado => "#6c757d",
                _ => "#6c757d"
            };
        }
    }
}