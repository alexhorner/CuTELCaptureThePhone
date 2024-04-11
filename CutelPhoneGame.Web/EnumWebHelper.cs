using CutelPhoneGame.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CutelPhoneGame.Web
{
    public static class EnumWebHelper
    {
        public static List<SelectListItem> EnumToSelectList<TEnum>() where TEnum : Enum => Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(v =>
        {
            string? displayName = v.GetDisplayNameFromValue();
            
            return new SelectListItem
            {
                Text = string.IsNullOrWhiteSpace(displayName) ? v.ToString() : displayName,
                Value = Convert.ToInt32(v).ToString()
            };
        }).ToList();
    }
}