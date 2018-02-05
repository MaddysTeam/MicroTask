using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pivotal.Discovery.Client;
using Microsoft.Extensions.Configuration;

namespace MicroTask.WorkTask
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
            // Add framework services.
            services.AddMvcCore(options =>
            {
                // add filter here
            })
            .AddAuthorization()
            .AddJsonFormatters();

            // Add Steeltoe.DiscoverClient.
            services.AddDiscoveryClient(this.Configuration);

            //auth service
            ConfigAuthService(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc(routes =>
            {
                // config routes
            });

            app.UseDiscoveryClient();

            //Route demo
            //RouteDemo(app);
        }


        //private void RouteService(IApplicationBuilder app)
        //{
        //    var trackPackageRouteHandler = new RouteHandler(async ctx =>
        //    {
        //        var routeValues = ctx.GetRouteData().Values;
        //    });

        //    var routeBuilder = new RouteBuilder(app, trackPackageRouteHandler);

        //    routeBuilder.MapGet("hello/{name}", builder =>
        //    {
        //        // var name = context.GetRouteValue("name");
        //        // This is the route handler when HTTP GET "hello/<anything>"  matches
        //        // To match HTTP GET "hello/<anything>/<anything>,
        //        // use routeBuilder.MapGet("hello/{*name}"
        //        // return context.Response.WriteAsync($"Hi, {name}!");
        //    });

        //    var routes2 = routeBuilder.Build();
        //    app.UseRouter(routes2);
        //}

        private void ConfigAuthService(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityUrl = Configuration.GetValue<string>("identityUrl");
            var apiName= Configuration.GetValue<string>("ApiName");
            services.AddAuthentication("Bearer")
                   .AddIdentityServerAuthentication(options => {
                       options.Authority = identityUrl;
                       options.RequireHttpsMetadata = false;
                       options.ApiName = apiName;
                   });
        }
    }
}
