using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using MAC.Servicios.Core.Modelos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using MAC.Servicios.Core.Contratos;
//using MAC.Servicios.Core.Implementacion;
//using MAC.Servicios.Core.Modelos.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using MAC.Servicios.Core.Api.Auth;

namespace ApiTrax
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            String currentPath = System.AppContext.BaseDirectory.ToLower();
            var builder = new ConfigurationBuilder().SetBasePath(currentPath).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IMACServices, ImplementServices>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ServiciosRest", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Servicios Trax Api Core",
                    Version = "1",
                    Description = "Documentaci�n de Trax Api Core"
                });
                options.CustomSchemaIds(type => type.ToString());
            });
            services.AddMvc(option => option.EnableEndpointRouting = false)
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
             .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddControllers();
            //services.AddSwaggerGen(x => {
            //    x.SwaggerDoc(name: "V1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Test swagger", Version = "V1" });
            //    var xmlFiles = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFiles);
            //    x.IncludeXmlComments(xmlPath);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(x => {
                x.SwaggerEndpoint(url: "/swagger/ServiciosRest/swagger.json","Trax Api Core");
                x.RoutePrefix =string.Empty;
            });
        }
    }
}
