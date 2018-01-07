using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BitFlagship
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            IWebHost host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .UseConfiguration(config)
                .UseUrls("http://0.0.0.0:5000/")
                .Build();

            host.Run();
        }
    }
}
