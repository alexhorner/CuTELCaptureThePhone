using CutelCaptureThePhone.Core.Attributes;

namespace CutelCaptureThePhone.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string? GetDisplayNameFromValue<TEnum>(this TEnum value) where TEnum : Enum
        {
            EnumDisplayNameAttribute? displayName = (EnumDisplayNameAttribute?)typeof(TEnum)
                .GetMember(value.ToString())[0]
                .GetCustomAttributes(false)
                .SingleOrDefault(a => a.GetType() == typeof(EnumDisplayNameAttribute));

            return displayName?.DisplayName;
        }
    }
}