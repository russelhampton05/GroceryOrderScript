using Newtonsoft.Json.Linq;
using System;

namespace KrogerApi
{
    public class KrogerConfig
    {
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Token;
        public string KrogerTokenUrl = "https://api.kroger.com/v1/connect/oauth2/token";
        public string KrogerCartUrl = "https://api.kroger.com/v1/cart/add";

        public void ToFile(string path)
        {
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            System.IO.File.WriteAllText(path, content);
        }

        public static KrogerConfig FromFile(string path)
        {
            var content = System.IO.File.ReadAllText(path);
            var jContent = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(content);
            KrogerConfig config = new KrogerConfig()
            {
                RefreshToken = jContent["RefreshToken"].ToString(),
                ClientId = jContent["ClientId"].ToString(),
                ClientSecret = jContent["ClientSecret"].ToString(),
                Token = jContent["Token"].ToString(),
                KrogerTokenUrl = jContent["KrogerTokenUrl"].ToString()
            };

            return config;

        }
    }

}
