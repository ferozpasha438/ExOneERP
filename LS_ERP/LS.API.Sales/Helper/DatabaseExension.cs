using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LS.API.Sales.Helper
{
    public static class DatabaseExension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddDbContext<CINServerDbContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("CINServerConnection"),
            //        b => b.MigrationsAssembly(typeof(CINServerDbContext).Assembly.FullName)));




            //services.AddDbContext<CINDBOneContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("CINDBOneConnection"),
            //        b => b.MigrationsAssembly(typeof(CINDBOneContext).Assembly.FullName)));


            //services.AddDbContext<DMCContext>(options =>
            //      options.UseSqlServer(
            //          configuration.GetConnectionString("DMCConnection"),
            //          b => b.MigrationsAssembly(typeof(DMCContext).Assembly.FullName)));

            //services.AddDbContext<DMC2Context>(options =>
            //               options.UseSqlServer(
            //                   configuration.GetConnectionString("DMC2Connection"),
            //                   b => b.MigrationsAssembly(typeof(DMC2Context).Assembly.FullName)));






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
