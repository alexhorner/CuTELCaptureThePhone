using System.Net.Mail;

namespace CutelPhoneGame.Core.Extensions
{
    public static class StringExtensions
    {
        public static string? EnsureValidUrl(this string? url)
        {
            url = url?.Trim();
            
            if (string.IsNullOrWhiteSpace(url) || url == "/" || !url.StartsWith("/") || !Uri.IsWellFormedUriString(url, UriKind.Relative)) return null;
            
            return url;
        }
    }
}