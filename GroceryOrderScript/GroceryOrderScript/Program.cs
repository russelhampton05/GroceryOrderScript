using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GroceryOrderScript
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var appSettings = config.GetSection("AppSettings");
            var defaultPath = appSettings["PathToGroceryList"];
            var pathToGecko = appSettings["PathToGecko"];
            ScriptLoader loader = new ScriptLoader();
            ScriptRunner runner = new ScriptRunner(pathToGecko);
            if (args.Length == 0)
            {
                Console.WriteLine($"No grocery list detected. Using default list location at {defaultPath}");
            }

            Console.WriteLine("Hello World!");
        }
    }
}
