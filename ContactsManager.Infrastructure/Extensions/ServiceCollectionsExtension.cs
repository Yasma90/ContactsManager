using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using ContactsManager.Persistence;
using ContactsManager.Persistence.ContextDb;
using ContactsManager.Persistence.Repository;
using ContactsManager.Persistence.Repository.Interfaces;
using ContactsManager.Domain.Models;

namespace ContactsManager.Infrastructure.Extensions
{
    public static class ServiceCollectionsExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ContactsDbContext>(opt => opt
                .UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(ContactsDbContext)
                                          .GetTypeInfo().Assembly.GetName().Name)));
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext)
                                          .GetTypeInfo().Assembly.GetName().Name)));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

    }
}
