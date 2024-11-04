namespace WebApplication4.Middelware
{
    using System.Net.Http.Headers;
    using System.Text;
    using Microsoft.AspNetCore.Http;

    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _username;
        private readonly string _password;

        public BasicAuthMiddleware(RequestDelegate next, string username, string password)
        {
            _next = next;
            _username = username;
            _password = password;
        }
        /*
         builder.Services.AddControllers(config =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    config.Filters.Add(new BasicAuthorizeAttribute(configuration));
});
         */
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers["WWW-Authenticate"] = "Basic";
                await context.Response.WriteAsync("Authorization header missing.");
                return;
            }

            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
            if (authHeader.Scheme != "Basic" || authHeader.Parameter == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Authorization header.");
                return;
            }

            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            if (credentials.Length != 2 || credentials[0] != _username || credentials[1] != _password)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }
          await  _next(context);
        }
    }

}
