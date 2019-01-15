/* 
*  Author: Kevin Suter
*  Description: This class is used to call the Microsoft Bing Web Search API.
*  
*/
using Microsoft.Extensions.Configuration;
using PersonalIntranetBot.Extensions;
using PersonalIntranetBot.Interfaces;
using PersonalIntranetBot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace PersonalIntranetBot.Helpers
{
    public class MicrosoftBingWebSearchService : IBingWebSearchService
    {
        private readonly string _accessKey;
        private readonly string _uriBase;
        private readonly int _searchDelay;

        public MicrosoftBingWebSearchService(IConfiguration configuration)
        {
            var bingOptions = new MicrosoftBingWebSearchOptions();
            configuration.Bind("BingWebSearchConfig", bingOptions);
            _accessKey = bingOptions.AccessKey;
            _uriBase = bingOptions.UriBase;
            _searchDelay = bingOptions.SearchDelay;
        }

        public MicrosoftBingWebSearchOptions BingWebSearchOptions
        {
            get => default(MicrosoftBingWebSearchOptions);
            set
            {
            }
        }

        /// <summary>
        /// Makes a request to the Bing Web Search API and returns data as a SearchResult.
        /// </summary>
        public BingJSONResult DoBingWebSearch(string searchQuery)
        {
            Thread.Sleep(_searchDelay);   

            // Construct the search request URI.
            var uriQuery = _uriBase + "?q=" + Uri.EscapeDataString(searchQuery);

            // Perform request and get a response.
            WebRequest request = WebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = _accessKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Create a result object.
            var searchResult = new BingJSONResult()
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
}


