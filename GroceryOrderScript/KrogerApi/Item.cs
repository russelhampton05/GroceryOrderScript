using Newtonsoft.Json;
using System.Collections.Generic;

namespace KrogerApi
{
    public class KrogerItemList
    {
        public List<Item> items { get; set; }
        public KrogerItemList(List<Item> items)
        {
            this.items = items;
        }
    }

    public class Item
    {
        [JsonProperty("upc")]
        public string UID { get; set; }
        [JsonProperty("quantity")]
        public int Count { get; set; }
    }

}
