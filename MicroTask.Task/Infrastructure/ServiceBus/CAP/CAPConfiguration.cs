using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{

    public static class CAPConfiguration
    {

        public static IServiceCollection AddCapWithMySQLAndRabbit(this IServiceCollection services, IConfiguration conifg)
        {
            services.AddDbContext<CAPDbContext>();

            // add cap
            services.AddCap(x =>
            {
                x.UseEntityFramework<CAPDbContext>();
                x.UseRabbitMQ("localhost");
            });

            return services;
        }

    }


    public class CAPDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=54.222.149.214;Database=microtask;User Id=root;Password=root;");
        }
    }

}
