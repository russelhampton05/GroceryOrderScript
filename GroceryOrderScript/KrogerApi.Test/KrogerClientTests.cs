using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace KrogerApi.Test
{
    [TestClass]
    public class KrogerClientTests
    {
        [TestMethod]
        public async Task KrogerClientGetsRefreshToken()
        {
            var krogerConfig = KrogerConfig.FromFile("../../../kroger_secrets.json");

            var client = new KrogerClient(krogerConfig);
            await client.RefreshToken();

        }
    }
}
