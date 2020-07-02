using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GroceryOrderScript
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var appSettings = config.GetSection("AppSettings");
                var pathToList = appSettings["PathToGroceryList"];
                var pathToGecko = appSettings["PathToGecko"];
                var pathToSecrets = appSettings["PathToSecret"];
                var pathToLoginScript = appSettings["PathToLoginScript"];
                var pathToCheckoutItemScript = appSettings["PathToCheckoutItemScript"];

                AppSecrets secrets = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSecrets>(File.ReadAllText(pathToSecrets));

                ScriptLoader loader = new ScriptLoader();
                using ScriptRunner runner = new ScriptRunner(pathToGecko);
                Console.Clear();
                runner.RunActions(loader.LoadLoginAction(pathToLoginScript, secrets.GroceryUsername, secrets.GroceryPassword, secrets.GroceryUrl));

                if (args.Length == 0)
                {
                    Console.WriteLine($"No grocery list detected. Using default list location at {pathToList}");
                }
                else
                {
                    Console.WriteLine($"Using {args[0]} as list");
                    pathToList = args[0];
                }

                var groceryItems = GetGroceryItems(pathToList);

                foreach (var item in groceryItems)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        var itemActions = loader.LoadGetItemAction(item, pathToCheckoutItemScript);
                        runner.RunActions(itemActions);
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }

            Console.WriteLine("End of script");
            Console.ReadLine();
        }

        private static List<GroceryItem> GetGroceryItems(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Select(line => new GroceryItem() { UID = line.Split(',')[0], Count = int.Parse(line.Split(',')[1]) })
                .ToList();
            
        }
    }
}
