using Discount.Api.Entities;
using Discount.Api.Extensions;
using Discount.Api.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Build Application  return IHost => to extend in it 
            var host =  CreateHostBuilder(args).Build(); 
            // Migrate Database 
            host.MigrateDatabase<Program>(ReflectionHelper.GetClassName<Coupon>());
            // Run Application 
            host.Run();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
