using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthorizationSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();

            services.AddAuthorization();

            services.AddTransient<IAuthorizationHandler, CustomerOperationAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, SupervisorOperationHandler>();
            services.AddTransient<IPermissionService, PermissionService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerfactory)
        {
            loggerfactory.AddConsole(LogLevel.Information);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true
            });

            app.Run(async context =>
            {
                context.Response.ContentType = "text/plain";
                var authorizationService = context.RequestServices.GetService<IAuthorizationService>();

                var users = new[]
                {
                    new[]
                    {
                        new Claim("id", "1"),
                        new Claim("name", "Alice"),
                        new Claim("email", "alice@smith.com"),
                        new Claim("status", "intern"),
                        new Claim("department", "sales"),
                        new Claim("region", "north")
                    },
                    new[]
                    {
                        new Claim("id", "2"),
                        new Claim("name", "Bob"),
                        new Claim("email", "bob@smith.com"),
                        new Claim("status", "senior"),
                        new Claim("department", "sales"),
                        new Claim("region", "north")
                    },
                    new[]
                    {
                        new Claim("id", "3"),
                        new Claim("name", "John"),
                        new Claim("email", "john@doe.com"),
                        new Claim("role", "supervisor"),
                        new Claim("department", "finance")
                    }
                }.Select(
                    c =>
                        new ClaimsPrincipal(new ClaimsIdentity(c, CookieAuthenticationDefaults.AuthenticationScheme,
                            "name", "role")));

                var operations = new[]
                {CustomerOperations.Manage, CustomerOperations.GiveDiscount(5), CustomerOperations.GiveDiscount(15)};

                var customers = new[]
                {
                    new Customer
                    {
                        Name = "Acme Corp",
                        Region = "north",
                        Fortune500 = false
                    },
                    new Customer
                    {
                        Name = "Bcme Corp",
                        Region = "south",
                        Fortune500 = false
                    },
                    new Customer
                    {
                        Name = "Jcme Corp",
                        Region = "north",
                        Fortune500 = true
                    }
                };

                foreach (var user in users)
                {
                    foreach (var operation in operations)
                    {
                        foreach (var customer in customers)
                        {
                            var isAuthorized = await authorizationService.AuthorizeAsync(user, customer, operation);
                            await context.Response.WriteAsync(
                                    $"user {user.FindFirst("name").Value,-15} isAuthorized: {isAuthorized,-15} for operation: {operation.Name,-15} with customer: {customer.Name,-15} {Environment.NewLine}");
                        }
                    }
                }
            });
        }
    }
}