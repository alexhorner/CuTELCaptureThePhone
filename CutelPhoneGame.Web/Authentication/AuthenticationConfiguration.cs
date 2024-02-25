namespace CutelPhoneGame.Web.Authentication
{
    public class AuthenticationConfiguration
    {
        public string SessionUserIdKey { get; set; } = null!;
        public string SessionStateCodeKey { get; set; } = null!;
        public string UnauthorisedErrorRedirectAction { get; set; } = null!;
        public string UnauthorisedRedirectActionController { get; set; } = null!;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(SessionUserIdKey)) throw new Exception($"{nameof(AuthenticationConfiguration)} error. {nameof(SessionUserIdKey)} cannot be null or whitespace.");
            if (string.IsNullOrWhiteSpace(SessionStateCodeKey)) throw new Exception($"{nameof(AuthenticationConfiguration)} error. {nameof(SessionStateCodeKey)} cannot be null or whitespace.");
            if (string.IsNullOrWhiteSpace(UnauthorisedErrorRedirectAction)) throw new Exception($"{nameof(AuthenticationConfiguration)} error. {nameof(UnauthorisedErrorRedirectAction)} cannot be null or whitespace.");
            if (string.IsNullOrWhiteSpace(UnauthorisedRedirectActionController)) throw new Exception($"{nameof(AuthenticationConfiguration)} error. {nameof(UnauthorisedRedirectActionController)} cannot be null or whitespace.");
        }
    }
}