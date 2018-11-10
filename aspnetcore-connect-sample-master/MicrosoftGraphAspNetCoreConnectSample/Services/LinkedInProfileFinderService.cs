using Newtonsoft.Json.Linq;
using PersonalIntranetBot.Helpers;
using PersonalIntranetBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Services
{
    class LinkedInSearchResult
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }

    public class LinkedInProfileFinderService
    {
        public static string LINKEDIN_SEARCH_SUFFIX = "linkedin";

        public static string GetLinkedInProfileURLFromNameAndCompany(string name, string company)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // add linked
            BingWebSearchService bingService = new BingWebSearchService();
            string searchString = String.Join(" ", new[] { name, company, LINKEDIN_SEARCH_SUFFIX });
            string bingWebSearchJSON = bingService.DoBingWebSearch(searchString).JsonResult;
            List<LinkedInSearchResult> linkedInSearchResults = ConvertBingSearchResultToLinkedInSearchResult(bingWebSearchJSON);
            /*
            foreach (LinkedInSearchResultItem item in linkedInSearchResults.Results)
            {
                Console.WriteLine(" *** Bing Web Search Result ***");
                Console.WriteLine("Id: " + item.Id);
                Console.WriteLine("Name: " + item.Description);
                Console.WriteLine("URL: " + item.URL);
            }
            */
            string bestMatch="";
            string LINKEDIN_URL_SEARCH_STRING = "/in/".ToLower();
            
            foreach (LinkedInSearchResult item in linkedInSearchResults)
            {
                bool isLinkedInProfilePage = item.URL.ToLower().Contains(LINKEDIN_URL_SEARCH_STRING.ToLower());
                bool isNameInDescription = item.Description.ToLower().Contains(name.ToLower());
                bool isCompanyInDescription = item.Description.ToLower().Contains(company.ToLower());
                // find first match
                if (String.IsNullOrEmpty(bestMatch) && isLinkedInProfilePage) {
                    bestMatch = item.URL.ToLower();
                }
                // find best match
                if (isNameInDescription && isCompanyInDescription && isLinkedInProfilePage)
                {
                    bestMatch = item.URL.ToLower();
                }
            }
            return bestMatch;
        }

        private static List<LinkedInSearchResult> ConvertBingSearchResultToLinkedInSearchResult(string json)
        {
            List<LinkedInSearchResult> results = new List<LinkedInSearchResult>();
            JObject jObject = JObject.Parse(json);
            JToken jRoot = jObject["webPages"];
            JToken[] values = jRoot["value"].ToArray();
            foreach (JToken value in values)
            {
                results.Add(new LinkedInSearchResult
                {
                    Id = (string)value["id"],
                    Description = (string)value["name"],
                    URL = (string)value["url"],
                });
            }
            return results;
        }

    }
}
