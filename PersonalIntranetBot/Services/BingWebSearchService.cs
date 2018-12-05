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
using static PersonalIntranetBot.Helpers.BingWebSearchService;
using PersonalIntranetBot.Extensions;
using Microsoft.Extensions.Configuration;

namespace PersonalIntranetBot.Helpers
{
    public class BingWebSearchService : IBingWebSearchService
    {
        private readonly string _accessKey;
        private readonly string _uriBase;

        public class BingSearchResult
        {
            public String JsonResult { get; set; }
            public Dictionary<String, String> RelevantHeaders { get; set; }
        }

        public BingWebSearchService(IConfiguration configuration)
        {
            var bingOptions = new BingOptions();
            configuration.Bind("BingWebSearchConfig", bingOptions);
            _accessKey = bingOptions.AccessKey;
            _uriBase = bingOptions.UriBase;
        }

        /// <summary>
        /// Makes a request to the Bing Web Search API and returns data as a SearchResult.
        /// </summary>
        public BingSearchResult DoBingWebSearch(string searchQuery)
        {
            // Construct the search request URI.
            var uriQuery = _uriBase + "?q=" + Uri.EscapeDataString(searchQuery);

            // Perform request and get a response.
            WebRequest request = WebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = _accessKey;
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
                if (header.StartsWith("BingAPIs-", StringComparison.Ordinal) || header.StartsWith("X-MSEdge-", StringComparison.Ordinal))
                    searchResult.RelevantHeaders[header] = response.Headers[header];
            }
            return searchResult;
        }
    }

    public interface IBingWebSearchService
    {
        BingSearchResult DoBingWebSearch(string searchQuery);
    }
}

