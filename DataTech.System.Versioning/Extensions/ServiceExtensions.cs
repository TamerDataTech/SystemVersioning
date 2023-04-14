using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DataTech.System.Versioning.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddEFSqlContext<TContext>(this IServiceCollection services, string connection) where TContext : DbContext
        {
            string assemblyName = Assembly.GetCallingAssembly().FullName;
            IConfiguration configuration;
            using (ServiceProvider provider = services.BuildServiceProvider())
            {
                configuration = provider.GetService<IConfiguration>();
            }

            services.AddTransient<SqlExecptionHandler>();
            return services.AddDbContext<TContext>(delegate (DbContextOptionsBuilder options)
            {
                options.UseSqlServer(configuration.GetConnectionString(connection), delegate (SqlServerDbContextOptionsBuilder assembly)
                {
                    assembly.MigrationsAssembly(assemblyName);
                });
            }, ServiceLifetime.Transient);
        }

    }
}
