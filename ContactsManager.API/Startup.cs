using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ContactsManager.Infrastructure.Extensions;
using ContactsManager.Persistence;

namespace ContacsManager.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            //services.AddSingleton(_ =>
            //{
            //    var configure = new AppConfiguration();
            //    Configuration.Bind(configure);

            //    return configure;
            //});
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContactsManager.API", Version = "v1" });
            });

            //var connectionString = Configuration.GetSection("DbConnection")?.Value ?? string.Empty;
            //var migrationsAssembly = typeof(ContactsDbContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddInfrastructure(Configuration.GetConnectionString("DbConnectionStr"));
            //services.AddDbContext<ContactsDbContext>();
            //services.AddDbContext<ContactsDbContext>(opt => opt
            //    .UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(ContactsDbContext)
            //                              .GetTypeInfo().Assembly.GetName().Name)));
            //services.AddTransient</*IContactRepository, */ContactRepository>();
            //services.AddTransient</*IUnitOfWork,*/ UnitOfWork>();

            //services.AddDbContext<ApplicationDbContext>(builder =>
            //    builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));
            //services.AddIdentity<User, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContactsManager.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
