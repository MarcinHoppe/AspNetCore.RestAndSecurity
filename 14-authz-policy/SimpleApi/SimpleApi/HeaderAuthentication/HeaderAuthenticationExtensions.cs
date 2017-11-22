using System;
using Microsoft.AspNetCore.Authentication;

namespace SimpleApi.HeaderAuthentication
{
    public static class HeaderAuthenticationExtensions
    {
        public static AuthenticationBuilder AddHeaderAuthentication(this AuthenticationBuilder builder,
            string authenticationScheme, string displayName, Action<HeaderAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<HeaderAuthenticationOptions, HeaderAuthenticationHandler>(authenticationScheme,
                displayName, configureOptions);
        }
    }
}