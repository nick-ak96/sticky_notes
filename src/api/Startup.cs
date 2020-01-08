using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using api.Models.Options;
using api.Repositories;
using api.Services;
using Microsoft.Extensions.Logging;

namespace api
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _config = configuration;
            _logger = loggerFactory.CreateLogger<Startup>();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.Add<api.Filters.GlobalExceptionHandler>();
                opt.Filters.Add<api.Filters.AuthorizationFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<AuthenticationKey>(_config.GetSection(nameof(AuthenticationKey)));
            services.Configure<DbContext>(_config.GetSection(nameof(DbContext)));

            // register services
            services.AddScoped<ITokenProviderService, TokenProviderService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<INoteService, NoteService>();

            // register db provider
            services.AddSingleton<IDBProvider, SQLiteProvider>();
            // register repos
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<IOrgNoteRepository, OrgNoteRepository>();
            services.AddScoped<IUserNoteRepository, UserNoteRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
