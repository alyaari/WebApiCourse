namespace WebApplication4.Attributes
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Net.Http.Headers;
    using System.Text;

    public class BasicAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private  string _username;
        private  string _password;

        public BasicAuthorizeAttribute(IConfiguration configuration)
        {
            _username = configuration["BasicAuth:Username"]??"";
            _password = configuration["BasicAuth:Password"] ?? "";
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var authHeader = AuthenticationHeaderValue.Parse(context.HttpContext.Request.Headers["Authorization"]);
            if (authHeader.Scheme != "Basic" || authHeader.Parameter == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            if (credentials.Length != 2 || credentials[0] != _username || credentials[1] != _password)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }

}
