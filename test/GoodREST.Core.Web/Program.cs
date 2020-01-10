using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.Extensions.Configuration;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile($"Config/hosting.json", optional: true, reloadOnChange: true)
                   .Build();

            var host = WebHost.CreateDefaultBuilder()

                .UseKestrel(x => { x.Configure(config.GetSection("Kestrel")); x.AllowSynchronousIO = true; })
                
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}