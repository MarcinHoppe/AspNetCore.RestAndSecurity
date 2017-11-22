using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SimpleApi.HeaderAuthentication
{
    public class HeaderAuthenticationHandler : AuthenticationHandler<HeaderAuthenticationOptions>
    {
        public HeaderAuthenticationHandler(IOptionsMonitor<HeaderAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var username = Request.Headers["username"].FirstOrDefault();
            var email = Request.Headers["email"].FirstOrDefault();
            var department = Request.Headers["department"].FirstOrDefault();

            if (!string.IsNullOrEmpty(username))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };

                if (!string.IsNullOrWhiteSpace(email))
                {
                    claims.Add(new Claim(ClaimTypes.Email, email));
                }
                if (!string.IsNullOrEmpty(department))
                {
                    claims.Add(new Claim("department", department));
                }

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "HeaderAuth"));

                var ticket = new AuthenticationTicket(
                    principal,
                    new AuthenticationProperties(),
                    Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Brak nagłówka username"));
        }
    }
}