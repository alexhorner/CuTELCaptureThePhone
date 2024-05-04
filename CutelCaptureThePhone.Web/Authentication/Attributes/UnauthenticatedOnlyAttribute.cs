using CutelCaptureThePhone.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CutelCaptureThePhone.Web.Authentication.Attributes
{
    public class UnauthenticatedOnlyAttribute : Attribute, IAuthorizationFilter, IFilterFactory
    {
        private readonly bool _jsonResponse;
        private readonly IAuthenticationManager _authenticationManager = null!;
        private readonly AuthenticationConfiguration _authenticationConfiguration = null!;
        public bool IsReusable => false;
        
        public UnauthenticatedOnlyAttribute(bool jsonResponse = false)
        {
            _jsonResponse = jsonResponse;
        }
        
        private UnauthenticatedOnlyAttribute(bool jsonResponse, IAuthenticationManager authenticationManager, AuthenticationConfiguration authenticationConfiguration)
        {
            _jsonResponse = jsonResponse;
            _authenticationManager = authenticationManager;
            _authenticationConfiguration = authenticationConfiguration;
        }
        
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            IServiceProvider scope = serviceProvider.CreateScope().ServiceProvider;

            IAuthenticationManager? authenticationManager = scope.GetRequiredService<IAuthenticationManager>();
            AuthenticationConfiguration? authenticationConfiguration = scope.GetRequiredService<AuthenticationConfiguration>();

            return new UnauthenticatedOnlyAttribute(_jsonResponse, authenticationManager, authenticationConfiguration);
        }
        
        public void OnAuthorization(AuthorizationFilterContext ctx)
        {
            if (!_authenticationManager.IsLoggedIn) return;
                
            if (_jsonResponse)
            {
                ctx.Result = new UnauthorizedObjectResult(new SimpleResponseModel
                {
                    Message = "You do not have permission to do this"
                });
            }
            else
            {
                ctx.Result = new RedirectToActionResult(_authenticationConfiguration.UnauthorisedErrorRedirectAction, _authenticationConfiguration.UnauthorisedRedirectActionController, new { Area = "" });
            }
        }
    }
}