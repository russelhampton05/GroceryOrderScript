using GoogleApiHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleApi.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetsSheetItems()
        {
            var pathToAppSecrets = "./Secrets/client_secret.json";
            var sheetId = "1AqvY1z2x7vWST3GDJJQVq9xhJZlfKcmm3HIPHYpz06I";
            var sheetName = "groceryList";

            GoogleSheetHelper sheetHelper = new GoogleSheetHelper(pathToAppSecrets, "GroceryApp");
            var items =  sheetHelper.GetRange(sheetId, $"{sheetName}!A2:C").Result;
        }
    }
}
