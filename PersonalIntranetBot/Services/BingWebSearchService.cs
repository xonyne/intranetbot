/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PersonalIntranetBot.Models;
using System.Text;
using System.Net;

namespace PersonalIntranetBot.Helpers
{
    public class BingWebSearchService
    {
        public class BingSearchResult
        {
            public String JsonResult { get; set; }
            public Dictionary<String, String> RelevantHeaders { get; set; }
        }

        // Enter a valid subscription key.
        // const string accessKey = "826920c686cb48fb8587647960f29103";
        // Enter a valid subscription key.
        const string accessKey = "aebed497b60d47d380526f1bb1f66d25";

        /*
         * If you encounter unexpected authorization errors, double-check this value
         * against the endpoint for your Bing Web search instance in your Azure
         * dashboard.
         */
        const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";

        /// <summary>
        /// Makes a request to the Bing Web Search API and returns data as a SearchResult.
        /// </summary>
        public virtual BingSearchResult DoBingWebSearch(string searchQuery)
        {
            // Construct the search request URI.
            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(searchQuery);

            // Perform request and get a response.
            WebRequest request = HttpWebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Create a result object.
            var searchResult = new BingSearchResult()
            {
                JsonResult = json,
                RelevantHeaders = new Dictionary<String, String>()
            };

            // Extract Bing HTTP headers.
            foreach (String header in response.Headers)
            {
                if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
                    searchResult.RelevantHeaders[header] = response.Headers[header];
            }
            return searchResult;
        }
    }
}

