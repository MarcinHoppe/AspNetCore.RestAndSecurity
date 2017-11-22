using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.Use((HttpContext context, Func<Task> next) =>
            {
                var username = context.Request.Headers["username"].FirstOrDefault();
                var email = context.Request.Headers["email"].FirstOrDefault();
                var department = context.Request.Headers["department"].FirstOrDefault();

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

                    context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "HeaderAuth"));
                }

                return next();
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}