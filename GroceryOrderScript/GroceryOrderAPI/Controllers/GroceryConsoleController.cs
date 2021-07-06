using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GroceryOrderScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using KrogerApi;
using GroceryOrderScript.KrogerExtensions;
namespace GroceryOrderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroceryController : ControllerBase
    {
        private readonly IConfiguration config;

        public GroceryController(IConfiguration config)
        {
            this.config = config;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var appSettings = config.GetSection("AppSettings");
                var pathToAppSecrets = appSettings["PathToAppSecrets"];
                var sheetId = appSettings["SheetId"];
                var sheetName = appSettings["SheetName"];
                var pathToKrogerConfig = appSettings["PathToKrogerSecret"];
                KrogerConfig krogerConfig = KrogerConfig.FromFile(pathToKrogerConfig);
                KrogerClient krogerClient = new KrogerClient(krogerConfig);
                GoogleSheetHelper sheetHelper = new GoogleSheetHelper(pathToAppSecrets, "GroceryApp");
                var items = await sheetHelper.GetRange(sheetId, $"{sheetName}!A2:C");
                var groceryItems = GetGroceryItems(items.Values);
                var kitems = groceryItems.Select(m => m.KrogerItemFromGroceryItem());
                await krogerClient.RefreshToken();
                krogerConfig.ToFile(pathToKrogerConfig);
                await krogerClient.Add(kitems.ToList());

                return Ok("Script ran successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

     
        private static List<GroceryItem> GetGroceryItems(IList<IList<object>> items)
        {
            List<GroceryItem> groceryItems = new List<GroceryItem>();
            foreach (var item in items)
            {
                if (item.Count != 3)
                {
                    continue;
                }

                groceryItems.Add(new GroceryItem() { UID = item[1].ToString(), Count = int.Parse(item[2].ToString()) });
            }

            return groceryItems;
        }
    }
}
