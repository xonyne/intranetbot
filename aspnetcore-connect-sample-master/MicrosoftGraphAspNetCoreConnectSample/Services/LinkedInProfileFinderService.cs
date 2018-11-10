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
    public class LinkedInProfileFinderService
    {
        public static Dictionary<string, string> getLinkedInProfileURLsFromEmailAddresses(String emailAddresses)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            string[] arrAddresses = emailAddresses.Split(',');
            foreach (string address in arrAddresses)
            {
                if (!String.IsNullOrEmpty(address))
                {
                    // get first part of email address and replace . by space (split first and last name)
                    string name = address.Split("@")[0].Replace(".", " ");
                    // get second part of email address and get only company name
                    string company = address.Split("@")[1].Split(".")[0];
                    string linkedInProfileURL = GetLinkedInProfileURLFromNameAndCompany(name, company);
                    // artificial slow down, because Bing does not allow more than 5 requests per second.
                    Thread.Sleep(500);
                    results.Add(name + "(" + company + ")", linkedInProfileURL);
                }
            }
            return results;
        }

        public static string GetLinkedInProfileURLFromNameAndCompany(string name, string company)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // add linked
            string linkedInSearchString = "linkedin";
            string searchString = String.Join(" ", new[] { name, company, linkedInSearchString });

            string bingWebSearchJSON = BingWebSearch.DoBingWebSearch(searchString).JsonResult;
            List<LinkedInProfileViewModel> linkedInSearchResults = ConvertBingSearchResultToLinkedInViewModel(bingWebSearchJSON);
            /*
            foreach (LinkedInSearchResultItem item in linkedInSearchResults.Results)
            {
                Console.WriteLine(" *** Bing Web Search Result ***");
                Console.WriteLine("Id: " + item.Id);
                Console.WriteLine("Name: " + item.Description);
                Console.WriteLine("URL: " + item.URL);
            }
            */

            foreach (LinkedInProfileViewModel item in linkedInSearchResults)
            {
                // we assume for now that all descriptions with the string "/in/" refer to a LinkedIn profile page
                string linkedInProfileSearchString = "/in/".ToLower();
                string personNameSearchString = name.Trim().Replace(" ", "-").ToLower();
                string searchResultURL = item.URL.ToLower();
                if (searchResultURL.Contains(personNameSearchString) && searchResultURL.Contains(linkedInProfileSearchString))
                {
                    return item.URL;
                }
            }
            return "";
        }

        private static List<LinkedInProfileViewModel> ConvertBingSearchResultToLinkedInViewModel(string json)
        {
            List<LinkedInProfileViewModel> results = new List<LinkedInProfileViewModel>();
            JObject jObject = JObject.Parse(json);
            JToken jRoot = jObject["webPages"];
            JToken[] values = jRoot["value"].ToArray();
            foreach (JToken value in values)
            {
                results.Add(new LinkedInProfileViewModel
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
