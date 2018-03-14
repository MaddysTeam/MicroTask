using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    
    /// <summary>
    /// cofiguration for .net core log extension
    /// </summary>
    public static class LogConfiguration
    {

        public static IApplicationBuilder UseNetCoreLogger(this IApplicationBuilder builder, ILoggerFactory factory, LogType logType,IConfiguration configuration)
        {
            var section = configuration.GetSection("Logging");
            switch (logType)
            {
                case LogType.Console:
                    factory = factory.AddConsole(section);break;
            }

            factory.AddDebug();

            return builder;
        }

    }

    public enum LogType
    {
        Console,
        DB,
        File
    }

}
