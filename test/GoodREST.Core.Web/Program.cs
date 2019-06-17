using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel((options) =>
                {
                    options.Listen(IPAddress.Loopback, 0);
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .UseStartup<Startup>().
                UseUrls()
                .Build();

            host.Start();
        }
    }
}