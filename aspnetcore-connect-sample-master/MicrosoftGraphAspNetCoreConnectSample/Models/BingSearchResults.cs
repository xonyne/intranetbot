using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Models
{
    public class BingSearchResults
    {
        public List<BingSearchResultItem> results { get; set; }

        public BingSearchResults(string json)
            {
                JObject jObject = JObject.Parse(json);
                JToken jRoot = jObject["webPages"];
                JToken[] values = jRoot["value"].ToArray();
                results = new List<BingSearchResultItem>();
                foreach (JToken value in values) {
                    results.Add(new BingSearchResultItem
                    {
                        Id = (string)value["id"],
                        Description = (string)value["name"],
                        URL = (string)value["url"],
                    });
                }
            }

        public class BingSearchResultItem
        {
            public string Id { get; set; }
            public string Description { get; set; }
            public string URL { get; set; }
        }
    }
}
