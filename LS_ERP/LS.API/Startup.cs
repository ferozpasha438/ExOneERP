using CIN.Application;
using FluentValidation.AspNetCore;
using LS.API.Helper;
using LS.API.HttpContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;

namespace LS.API.HttpContext
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

namespace LS.API
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
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("myconfig",
            //    builder =>
            //    {
            //        builder.WithOrigins("https://localhost:44364")
            //                            //"http://www.contoso.com")
            //                            .AllowAnyHeader()
            //                            .AllowAnyMethod();
            //    });
            //});            

            services.AddLocalization();
            services.Configure<AppSettingsJson>(Configuration.GetSection("AppSettings"));
            //services.AddLocalization(options => options.ResourcesPath = "ViewResources");
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ar-SA");
            //    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("ar-SA") };
            //    //options.RequestCultureProviders.Insert(0, new CustomerCultureProvider());
            //});

            services.AddMemoryCache();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new CultureInfo[] {
            new CultureInfo("en-US"),
            new CultureInfo("ar")
            //new CultureInfo("ar-SA")
    };
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders = new[]{ new RouteDataRequestProvider{
            IndexOfCulture=1,
        }};
            });


            services.AddMvc();
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            //services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews(options =>
               options.Filters.Add<ApiExceptionFilterAttribute>())
                   .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var signingKey = Convert.FromBase64String(Configuration["Jwt:Key"]);
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                };
            });
            services.AddCors();
            // services.AddHttpContextAccessor();

            //services.AddControllers();
            //services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            // logger.AddLog4Net();

            if (env.IsDevelopment())
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo($"{env.ContentRootPath}\\log4net.config"));
                app.UseDeveloperExceptionPage();
            }

            ////var supportedCultures = new[]
            ////          {
            ////            new CultureInfo("en-US"),
            ////            new CultureInfo("ar"),
            ////           };

            ////app.UseRequestLocalization(new RequestLocalizationOptions
            ////{
            ////    ApplyCurrentCultureToResponseHeaders = true,
            ////    DefaultRequestCulture = new RequestCulture("en-US"),
            ////    // Formatting numbers, dates, etc.
            ////    SupportedCultures = supportedCultures,
            ////    // UI strings that we have localized.
            ////    SupportedUICultures = supportedCultures
            ////});

            ////app.Use((context, next) =>
            ////{
            ////    var userLangs = context.Request.Headers["Accept-Language"].ToString();
            ////    var lang = userLangs.Split(',').FirstOrDefault();

            ////    //If no language header was provided, then default to english.
            ////    if (string.IsNullOrEmpty(lang))
            ////    {
            ////        lang = "en-US";
            ////    }

            ////    //You could set the environment culture based on the language.

            ////    //// context.Response.Cookies.Append(
            ////    ////CookieRequestCultureProvider.DefaultCookieName,
            ////    ////CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)));

            ////    //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
            ////    //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            ////    //you could save the language preference for later use as well.
            ////    context.Items["SelectedLng"] = lang;
            ////    //context.Items["ClientCulture"] = Thread.CurrentThread.CurrentUICulture.Name;


            ////    return next();
            ////});

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);


            //  app.UseHttpsRedirection();



            //app.UseCors(options =>
            //             options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            //            );


            // global cors policy
            app.UseCors(x =>
                //  x.AllowAnyOrigin()
                //x.WithOrigins("http://localhost:43318")
                x.WithOrigins("http://shamimmn-002-site10.itempurl.com/", "http://20.68.125.92/ErpUi", "http://localhost:43318")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticHttpContext();
        }
    }
}
