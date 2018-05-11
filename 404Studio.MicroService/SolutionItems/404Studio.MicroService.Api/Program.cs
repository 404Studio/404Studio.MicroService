using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using YH.Etms.Settlement.Api.Infrastructure;
using YH.Framework.WebHostExtensions;

namespace YH.Etms.Settlement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<SettlementContext>((context, services) =>
                {
                    var logger = services.GetService<ILogger<SettlementContextSeed>>();
                    var env = services.GetService<IHostingEnvironment>();
                    new SettlementContextSeed()
                        .SeedAsync(context, env, logger)
                        .Wait();

                })
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:8002")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .UseNLog()
                .Build();
    }
}
