using System.Net.Mail;

namespace CutelCaptureThePhone.Core.Extensions
{
    public static class StringExtensions
    {
        public static string? EnsureValidUrl(this string? url)
        {
            url = url?.Trim();
            
            if (string.IsNullOrWhiteSpace(url) || url == "/" || !url.StartsWith("/") || !Uri.IsWellFormedUriString(url, UriKind.Relative)) return null;
            
            return url;
        }
        
        public static string ToUpperFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            
            char[] characters = str.ToCharArray();
            
            characters[0] = char.ToUpper(characters[0]);
            
            return new string(characters);
        }
    }
}