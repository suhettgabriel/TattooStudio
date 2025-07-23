using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    }
}