using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TestProject.Infrastructure.Database;

namespace TestProject.API.ApiExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder app, IConfiguration configuration, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                Log.Information("Starting initialization");

                using (var context = serviceScope.ServiceProvider.GetService<DefaultContext>())
                {
                    if (context == null)
                        throw new Exception($"Context was not created! (in {nameof(UseDbInitializer)})");

                    Log.Information("Created context");

                    if (!env.IsTest())
                    {
                        context.Database.EnsureCreated();
                        context.GetService<IMigrator>().Migrate();
                    }

                    Log.Information("Initialization has been completed");

                    return app;
                }
            }
        }
    }
}
