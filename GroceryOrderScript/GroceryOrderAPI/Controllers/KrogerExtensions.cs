using GroceryOrderScript;
using KrogerApi;

namespace GroceryOrderAPI.Controllers
{
    static class KrogerExtensions
    {
        public static Item KrogerItemFromGroceryItem(this GroceryItem groceryItem)
        {
            Item kitem = new Item()
            {
                UID = groceryItem.UID,
                Count = groceryItem.Count
            };

            return kitem;
        }
    }
}