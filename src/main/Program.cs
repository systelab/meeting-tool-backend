namespace Main
{
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost();
            host.Run();
            // BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost()
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory()).UseUrls("http://0.0.0.0:13080")
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
        }
        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();


    }
}