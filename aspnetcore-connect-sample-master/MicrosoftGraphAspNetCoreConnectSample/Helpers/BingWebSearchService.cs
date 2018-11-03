/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PersonalIntranetBot.Models;
using System.Text;
using System.Net;
using static PersonalIntranetBot.Models.BingSearchResults;

namespace PersonalIntranetBot.Helpers
{
    public static class BingWebSearchService
    {
        // Enter a valid subscription key.
        const string accessKey = "826920c686cb48fb8587647960f29103";
        /*
         * If you encounter unexpected authorization errors, double-check this value
         * against the endpoint for your Bing Web search instance in your Azure
         * dashboard.
         */
        const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";

        // Returns search results with headers.
        struct SearchResult
        {
            public String jsonResult;
            public Dictionary<String, String> relevantHeaders;
        }

        /// <summary>
        /// Makes a request to the Bing Web Search API and returns data as a SearchResult.
        /// </summary>
        static SearchResult BingWebSearch(string searchQuery)
        {
            // Construct the search request URI.
            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(searchQuery);

            // Perform request and get a response.
            WebRequest request = HttpWebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Create a result object.
            var searchResult = new SearchResult()
            {
                jsonResult = json,
                relevantHeaders = new Dictionary<String, String>()
            };

            // Extract Bing HTTP headers.
            foreach (String header in response.Headers)
            {
                if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
                    searchResult.relevantHeaders[header] = response.Headers[header];
            }
            return searchResult;
        }

        public static string getLinkedInProfileURLFromNameAndCompany(string name, string company)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (accessKey.Length == 32)
            {
                // add linked
                string linkedInSearchString = "linkedin";
                string searchString = String.Join(" ", new[] { name, company, linkedInSearchString });
                Console.WriteLine("Searching the Web for: " + searchString);
                SearchResult result = BingWebSearch(searchString);
                Console.WriteLine("\nRelevant HTTP Headers:\n");
                foreach (var header in result.relevantHeaders)
                    Console.WriteLine(header.Key + ": " + header.Value);

                Console.WriteLine("\nJSON Response:\n");
                BingSearchResults bingSearchResult = new BingSearchResults(result.jsonResult);
                
                foreach (BingSearchResultItem item in bingSearchResult.results)
                {
                    Console.WriteLine(" *** Bing Web Search Result ***");
                    Console.WriteLine("Id: " + item.Id);
                    Console.WriteLine("Name: " + item.Description);
                    Console.WriteLine("URL: " + item.URL);
                }
                
                return findLinkedInProfileURLFromBingSearchResults(bingSearchResult, name);
            }
            else
            {
                Console.WriteLine("Invalid Bing Search API subscription key!");
                Console.WriteLine("Please paste yours into the source code.");
            }
            return "";
        }

        static string findLinkedInProfileURLFromBingSearchResults(BingSearchResults searchResult, String personName) {
            foreach (BingSearchResultItem item in searchResult.results) {
                // we assume for now that all descriptions with this string in it refer to a profile page
                string linkedInProfileSearchString = "/in/".ToLower();
                string personNameSearchString = personName.Replace(" ", "-").ToLower();
                string searchResultURL = item.URL.ToLower();
                if (searchResultURL.Contains(personNameSearchString) && searchResultURL.Contains(linkedInProfileSearchString)) {
                    return item.URL;
                }
            }
            return "";
        }

    }
}

