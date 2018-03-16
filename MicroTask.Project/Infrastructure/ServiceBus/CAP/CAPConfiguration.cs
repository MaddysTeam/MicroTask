using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{

    public static class CAPConfiguration
    {

        static IConfiguration configuration;

        public static IServiceCollection AddCapWithMySQLAndRabbit(this IServiceCollection services, IConfiguration conifg)
        {
            configuration = conifg;

            services.AddDbContext<CAPDbContext>();

            // add cap
            services.AddCap(x =>
            {
                x.UseEntityFramework<CAPDbContext>();
                x.UseRabbitMQ("localhost");
            });

            return services;
        }



        public class CAPDbContext : DbContext
        {
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseMySql("Database=microtask;Data Source=54.222.149.214;User Id=root;Password=root;pooling=false;CharSet=utf8;port=3306");
            }
        }

    }



}
