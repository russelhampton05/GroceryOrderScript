using KrogerApi;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoogleApiHelpers;
using GroceryOrderScript.KrogerExtensions;

namespace GroceryOrderScript
{

    public class Program
    {
        public static async Task Main(string[] args)
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
                var pathToSecrets = appSettings["PathToSecret"];
                var pathToKrogerConfig = appSettings["PathToKrogerSecret"];
                AppSecrets secrets = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSecrets>(File.ReadAllText(pathToSecrets));
                KrogerConfig krogerConfig = KrogerConfig.FromFile(pathToKrogerConfig);
                KrogerClient krogerClient = new KrogerClient(krogerConfig);
                GoogleSheetHelper sheetHelper = new GoogleSheetHelper(pathToAppSecrets, "GroceryApp");
                var items = await sheetHelper.GetRange(sheetId, $"{sheetName}!A:B");
                var groceryItems = GetGroceryItems(items.Values);
                var kitems = groceryItems.Select(m => m.KrogerItemFromGroceryItem());
                await krogerClient.RefreshToken();
                krogerConfig.ToFile(pathToKrogerConfig);
                await krogerClient.Add(kitems.ToList());
                Console.WriteLine("All done");

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
 