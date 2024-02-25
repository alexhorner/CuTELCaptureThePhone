using CutelPhoneGame.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace CutelPhoneGame.Web.ApiAuthentication.Attributes
{
    public class ApiAuthenticatedOnlyAttribute : Attribute, IAuthorizationFilter, IFilterFactory
    {
        private readonly IConfiguration _configuration = null!;
        public bool IsReusable => false;

        public ApiAuthenticatedOnlyAttribute()
        {
        }
        
        private ApiAuthenticatedOnlyAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            IServiceProvider scope = serviceProvider.CreateScope().ServiceProvider;

            IConfiguration configuration = scope.GetRequiredService<IConfiguration>();

            return new ApiAuthenticatedOnlyAttribute(configuration);
        }
        
        public void OnAuthorization(AuthorizationFilterContext ctx)
        {
            if (!ctx.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues auth))
            {
                ctx.Result = new UnauthorizedObjectResult(new SimpleResponseModel
                {
                    Message = "No Authorization header provided"
                });

                return;
            }
            
            if(!(auth[0] ?? "").StartsWith("Bearer "))
            {
                ctx.Result = new UnauthorizedObjectResult(new SimpleResponseModel
                {
                    Message = "Authorization header is not a bearer token"
                });

                return;
            }
            
            string token = auth[0]!.Split(" ", 2)[1];

            if (token != _configuration.GetValue<string>("ApiKey"))
            {
                ctx.Result = new UnauthorizedObjectResult(new SimpleResponseModel
                {
                    Message = "Authorization is incorrect"
                });
            }
        }
    }
}