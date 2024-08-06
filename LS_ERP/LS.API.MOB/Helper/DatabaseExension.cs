using CIN.DB;
using CIN.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace LS.API.Helper
{
    public static class DatabaseExension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<CINServerDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("CINServerConnection"),
                    b => b.MigrationsAssembly(typeof(CINServerDbContext).Assembly.FullName)));

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddDbContext<CINDBOneContext>((serviceProvider, dbContextBuilder) =>
            //{
            //    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            //    var connectionString = httpContextAccessor.HttpContext.Request.Headers["ConnectionString"].FirstOrDefault();
            //    if (connectionString is not null && !string.IsNullOrEmpty(connectionString))
            //    {
            //        byte[] b = System.Convert.FromBase64String(connectionString);
            //        string dbConnetion = System.Text.ASCIIEncoding.ASCII.GetString(b);
            //        // dbConnetion = $"{dbConnetion.Replace(@"\\\\", @"\\")}";
            //        dbContextBuilder.UseSqlServer($"{dbConnetion.Replace(@"\\", @"\")}");
            //    }
            //});


            services.AddDbContext<CINDBOneContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("CINDBOneConnection"),
                    b => b.MigrationsAssembly(typeof(CINDBOneContext).Assembly.FullName)));

            services.AddDbContext<DMCContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DMCConnection"),
                    b => b.MigrationsAssembly(typeof(DMCContext).Assembly.FullName)));

            services.AddDbContext<DMC2Context>(options =>
                           options.UseSqlServer(
                               configuration.GetConnectionString("DMC2Connection"),
                               b => b.MigrationsAssembly(typeof(DMC2Context).Assembly.FullName)));



            //services.AddDbContext<CINDBTwoContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("CINDBTwoConnection"),
            //        b => b.MigrationsAssembly(typeof(CINDBTwoContext).Assembly.FullName)));

            //services.AddDbContext<CINDBThreeContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("CINDBThreeConnection"),
            //        b => b.MigrationsAssembly(typeof(CINDBThreeContext).Assembly.FullName)));


            #region Not Generated Migration for this Context Class

            //services.AddDbContext<CINDBFourContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("CINDBFourConnection"),
            //        b => b.MigrationsAssembly(typeof(CINDBFourContext).Assembly.FullName)));
            #endregion


            return services;
        }


    }
}
