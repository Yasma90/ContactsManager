using ContactsManager.Persistence.Help;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Infrastructure.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void AddConfigureDbInitializer(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
                if (dbInitializer != null)
                {
                    dbInitializer.Initialize();
                    dbInitializer.SeedData();
                }
            }
        }

        public static void AddConfigureAuthenticationDb(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
                if (dbInitializer != null)
                {
                    dbInitializer.SeedDataUser();
                }
            }

        }
    }
}
