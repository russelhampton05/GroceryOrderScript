using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryOrderScript
{

    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                
                var appSettings = config.GetSection("AppSettings");
                var pathToAppSecrets = appSettings["PathToAppSecrets"];
                var sheetId = appSettings["SheetId"];
                var sheetName = appSettings["SheetName"];
                var pathToGecko = appSettings["PathToGecko"];
                var pathToSecrets = appSettings["PathToSecret"];
                var pathToLoginScript = appSettings["PathToLoginScript"];
                var pathToCheckoutItemScript = appSettings["PathToCheckoutItemScript"];

                AppSecrets secrets = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSecrets>(File.ReadAllText(pathToSecrets));
                GoogleSheetHelper sheetHelper = new GoogleSheetHelper(pathToAppSecrets, "GroceryApp");
                var items = await sheetHelper.GetRange(sheetId, $"{sheetName}!A:B");
                var groceryItems = GetGroceryItems(items.Values);
                ScriptLoader loader = new ScriptLoader();
                using ScriptRunner runner = new ScriptRunner(pathToGecko);

                runner.RunActions(loader.LoadLoginAction(pathToLoginScript, secrets.GroceryUsername, secrets.GroceryPassword, secrets.GroceryUrl));

             

                foreach (var item in groceryItems)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        var itemActions = loader.LoadGetItemAction(item, pathToCheckoutItemScript);
                        runner.RunActions(itemActions);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR : {ex.Message}");
            }

            Console.WriteLine("End of script");
            Console.ReadLine();
        }

        private static List<GroceryItem> GetGroceryItems(IList<IList<object>> items)
        {
            List<GroceryItem> groceryItems = new List<GroceryItem>();
            foreach (var item in items)
            {
                if(item.Count != 2)
                {
                    continue;
                }

                groceryItems.Add(new GroceryItem() { UID = item[0].ToString(), Count = int.Parse(item[1].ToString()) });
            }

            return groceryItems;
        }
    }
}
