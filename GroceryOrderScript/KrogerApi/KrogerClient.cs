using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KrogerApi
{

    public class KrogerClient
    {
        private KrogerConfig config;

        public KrogerClient(KrogerConfig config)
        {
            this.config = config;
        }

        public KrogerConfig GetConfig()
        {
            return config;
        }

        /// <summary>
        /// Refreshes config in place and returns a copy of the new tokens
        /// </summary>
        public async Task<(string Token, string RefreshToken)> RefreshToken()
        {
            string authString = $"{config.ClientId}:{config.ClientSecret}";
            authString = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));
           
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(config.KrogerTokenUrl);
            requestMessage.Content = new StringContent($"grant_type=refresh_token&refresh_token={config.RefreshToken}");
            requestMessage.Method = HttpMethod.Post;
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var result = await MakeCall(requestMessage);
            var jContent = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(await result.Content.ReadAsStringAsync());

            this.config.Token = jContent["access_token"].ToString();
            this.config.RefreshToken = jContent["refresh_token"].ToString();

            return (this.config.Token, this.config.RefreshToken);
        }

        public async Task Add(List<Item> items)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(config.KrogerCartUrl);
            var test = JsonConvert.SerializeObject(new KrogerItemList(items));
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new KrogerItemList(items)));
            requestMessage.Method = HttpMethod.Put;
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.Token);
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            
            await MakeCall(requestMessage);
        }

        private async Task<HttpResponseMessage> MakeCall(HttpRequestMessage message)
        {
            HttpClient client = new HttpClient();
            var result = await client.SendAsync(message);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"{result.StatusCode} {result.ReasonPhrase} {result.Content.ToString()}");
            }

            return result;
        }

    }

}
