using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApiChat.Helpers;
using WebApiChat.Models;

namespace WebApiChat
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
            var appsetting = Configuration.Get<AppSettings>();

            services.AddSingleton<IAppSettings>(appsetting);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();

            services.AddDbContext<AuthenticationContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });
            services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<AuthenticationContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });
            services.AddCors(options => options.AddPolicy("AllowWebApp", builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));

            services.AddSwaggerGen(c =>
            {
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();

                c.SwaggerDoc(appsetting.VersionInfo.VersionNumber, new OpenApiInfo { Title = appsetting.AppDescription, Version = appsetting.VersionInfo.VersionNumber });
                c.MapType<JObject>(() => new OpenApiSchema { Type = "object" });
                string docxmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string fullPath = Path.Combine(AppContext.BaseDirectory, docxmlFile);

                c.IncludeXmlComments(fullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAppSettings appSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint($"./swagger/{appSettings.VersionInfo.VersionNumber}/swagger.json", appSettings.ApplicationName);
                s.DocExpansion(DocExpansion.List);
                s.RoutePrefix = string.Empty;
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseCors("AllowWebApp");


            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
