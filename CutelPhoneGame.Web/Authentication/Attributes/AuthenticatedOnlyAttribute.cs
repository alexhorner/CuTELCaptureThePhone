using CutelPhoneGame.Core.Extensions;
using CutelPhoneGame.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CutelPhoneGame.Web.Authentication.Attributes
{
    public class AuthenticatedOnlyAttribute : Attribute, IAuthorizationFilter, IFilterFactory
    {
        private readonly bool _jsonResponse;
        private readonly bool _suppressMessages;
        private readonly IAuthenticationManager _authenticationManager = null!;
        private readonly AuthenticationConfiguration _authenticationConfiguration = null!;
        public bool IsReusable => false;

        public AuthenticatedOnlyAttribute(bool jsonResponse = false, bool suppressMessages = false)
        {
            _jsonResponse = jsonResponse;
            _suppressMessages = suppressMessages;
        }
        
        private AuthenticatedOnlyAttribute(bool jsonResponse, bool suppressMessages, IAuthenticationManager authenticationManager, AuthenticationConfiguration authenticationConfiguration)
        {
            _jsonResponse = jsonResponse;
            _suppressMessages = suppressMessages;
            _authenticationManager = authenticationManager;
            _authenticationConfiguration = authenticationConfiguration;
        }
        
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            IServiceProvider scope = serviceProvider.CreateScope().ServiceProvider;

            IAuthenticationManager authenticationManager = scope.GetRequiredService<IAuthenticationManager>();
            AuthenticationConfiguration authenticationConfiguration = scope.GetRequiredService<AuthenticationConfiguration>();

            return new AuthenticatedOnlyAttribute(_jsonResponse, _suppressMessages, authenticationManager, authenticationConfiguration);
        }
        
        public void OnAuthorization(AuthorizationFilterContext ctx)
        {
            if (_authenticationManager.IsLoggedIn) return;

            if (_jsonResponse)
            {
                ctx.Result = new UnauthorizedObjectResult(new SimpleResponseModel
                {
                    Message = "You do not have permission to do this"
                });
            }
            else
            {
                string? returnUrl = ctx.HttpContext.Request.Path.ToUriComponent();
                string queryString = ctx.HttpContext.Request.QueryString.ToUriComponent();

                if (!string.IsNullOrWhiteSpace(returnUrl) && !string.IsNullOrWhiteSpace(queryString)) returnUrl += queryString;
                
                ctx.Result = new RedirectToActionResult(_authenticationConfiguration.UnauthorisedErrorRedirectAction, _authenticationConfiguration.UnauthorisedRedirectActionController, new { Area = "", SuppressMessages = _suppressMessages, ReturnUrl = returnUrl.EnsureValidUrl() });
            }
        }
    }
}