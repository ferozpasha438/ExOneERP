using CIN.Application;
using CIN.DB;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LS.API.Fin.HttpContext
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
    }
    public static class StaticHttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}

namespace LS.API.Zatca
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
            services.Configure<AppSettingsJson>(Configuration.GetSection("AppSettings"));

            services.AddMvc();
            services.AddApplication();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<DMCContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DMCConnection"),
                    b => b.MigrationsAssembly(typeof(DMCContext).Assembly.FullName)));

            services.AddDbContext<DMC2Context>(options =>
                           options.UseSqlServer(
                               Configuration.GetConnectionString("DMC2Connection"),
                               b => b.MigrationsAssembly(typeof(DMC2Context).Assembly.FullName)));

            services.AddDbContext<CINDBOneContext>((serviceProvider, dbContextBuilder) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var connectionString = httpContextAccessor.HttpContext.Request.Headers["ConnectionString"].FirstOrDefault();
                if (connectionString is not null && !string.IsNullOrEmpty(connectionString))
                {
                    byte[] b = System.Convert.FromBase64String(connectionString);
                    string dbConnetion = System.Text.ASCIIEncoding.ASCII.GetString(b);
                    // dbConnetion = $"{dbConnetion.Replace(@"\\\\", @"\\")}";
                    dbContextBuilder.UseSqlServer($"{dbConnetion.Replace(@"\\", @"\")}");
                }
            });

            services.AddControllersWithViews(options =>
              options.Filters.Add<ApiExceptionFilterAttribute>())
                  .AddFluentValidation(x => x.AutomaticValidationEnabled = false);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LS.API.Zatca", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LS.API.Zatca v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
