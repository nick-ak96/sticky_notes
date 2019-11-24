using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using api.Models.Options;
using api.Repositories;
using api.Services;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.Add<api.Filters.GlobalExceptionHandler>();
                opt.Filters.Add<api.Filters.AuthorizationFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<AuthenticationKey>(Configuration.GetSection(nameof(AuthenticationKey)));
            services.Configure<DbContext>(Configuration.GetSection(nameof(DbContext)));

            // register services
            services.AddScoped<ITokenProviderService, TokenProviderService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INoteService, NoteService>();

            // register db provider
            services.AddSingleton<IDBProvider, SQLiteProvider>();
            // register repos
            services.AddScoped<IUserRepository, UserRepository>();
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
