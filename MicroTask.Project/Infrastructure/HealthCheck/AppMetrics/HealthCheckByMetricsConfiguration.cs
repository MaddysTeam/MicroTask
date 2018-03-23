using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Reporting.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{

    public static class HealthCheckByMetricsConfiguration
    {

        public static void AddHealthConfigurationByMetrics(this IServiceCollection serivce,IConfiguration config)
        {
            var db = config.GetSection("HealthCheck:InfluxDB:Name").Value;
            var url = new Uri(config.GetSection("HealthCheck:InfluxDB:Url").Value);

            serivce.AddMetrics(options =>
            {
                options.GlobalTags.Add("app", "project");
                options.GlobalTags.Add("env", "stage");
            })
            .AddHealthChecks()
            .AddJsonSerialization()
            .AddReporting(factory =>
            {
                factory.AddInfluxDb(new InfluxDBReporterSettings
                {
                    InfluxDbSettings=new InfluxDBSettings(db, url)
                });
            })
            .AddMetricsMiddleware(options => options.IgnoredHttpStatusCodes = new[] { 404});
        }


        public static void UseHealthConfigurationByMetrics(this IApplicationBuilder app, IApplicationLifetime lifeTime)
        {
            app.UseMetrics();
            app.UseMetricsReporting(lifeTime);
        }

    }

}
