using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContactsManager.Domain.Models;
using ContactsManager.Persistence.ContextDb;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ContactsManager.Domain;

namespace ContactsManager.Persistence.Help
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly AppConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            AppConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public void Initialize()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ContactsDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ContactsDbContext>())
                {
                    if (!context.Contacts.Any())
                    {
                        var contact = new Contact
                        {
                            FirstName = "Jorge",
                            LastName = "Ramos",
                            Email = "jramos@sgiadmin.cu",
                            DateOfBirth = DateTime.Parse("14-04-1989"),
                            Phone = "+5355555555"
                        };

                        context.Contacts.Add(contact);
                    }

                    context.SaveChanges();
                }
            }
        }

        #region Authentication

        public void SeedDataUser()
        {
            if (SeedRoles("Admin"))
                SeedUsers();
            //SeedRoles("Admin GSI");
        }

        private void SeedUsers()
        {
            var roleName = _configuration.AdminUser.Role;
            var username = _configuration.AdminUser.UserName;
            if (_userManager.FindByNameAsync(username).Result == null)
            {
                var user = new User
                {
                    UserName = username,
                    Email = _configuration.AdminUser.Email,
                    FirstName = _configuration.AdminUser.FirstName,
                    LastName = _configuration.AdminUser.LastName,
                    Password = _configuration.AdminUser.Password
                };

                try
                {
                    var result = _userManager.CreateAsync(user, user.Password).Result;

                    if (result.Succeeded)
                    {
                        _userManager.AddToRoleAsync(user, roleName).Wait();
                    }
                }
                catch (Exception ex) 
                {

                    throw new Exception(ex.Message);
                }
            }
        }

        private bool SeedRoles(string rolename)
        {
            if (!_roleManager.RoleExistsAsync(rolename).Result)
            {
                var role = new IdentityRole(rolename);
                return _roleManager.CreateAsync(role).Result.Succeeded;
            }
            return false;
        }

        #endregion
    }
}
