using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MiniLyokoV2
{
    public class Program
    {
        public static int Port;
        public static void Main(string[] args)
        {
            var webhost = CreateWebHostBuilder(args).Build();

            string adress = webhost.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First(adresss => !adresss.Contains("https"));
            string portstring = adress.Split(':')[2];
            Console.WriteLine($"adress: {adress} portstring: {portstring}");
            int port = Int32.Parse(portstring);
            Port = port;
            webhost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}