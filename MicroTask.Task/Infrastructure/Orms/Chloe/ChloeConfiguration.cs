using Chloe.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;

namespace Infrastructure
{

    public static class ChloeConfiguration 
    {

        public static IServiceCollection AddChloeWithMySQL(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(provider => {
                string connString = configuration.GetSection("ConnectStrings:MySql").Value;

                return new Chloe.MySql.MySqlContext(new MysqlConnectionFactory(connString));
            });

            return services;
        }

    }

}