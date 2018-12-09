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
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1.Data;

namespace PersonalIntranetBot.Helpers
{
    public class GoogleCustomSearchService : IGoogleCustomSearchService
    {
        private readonly string _apiKey;
        private readonly string _searchEngineId;

        public class BingSearchResult
        {
            public String JsonResult { get; set; }
            public Dictionary<String, String> RelevantHeaders { get; set; }
        }

        public GoogleCustomSearchService(IConfiguration configuration)
        {
            var googleOptions = new GoogleOptions();
            configuration.Bind("GoogleCustomSearchConfig", googleOptions);
            _apiKey = googleOptions.AccessKey;
            _searchEngineId = googleOptions.SearchEngineId;
        }

        /// <summary>
        /// Makes a request to the Bing Web Search API and returns data as a SearchResult.
        /// </summary>
        public string DoGoogleImageSearch(string searchQuery)
        {
            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = _apiKey });
            var listRequest = customSearchService.Cse.List(searchQuery);
            listRequest.Cx = _searchEngineId;
            listRequest.ImgType = CseResource.ListRequest.ImgTypeEnum.Face;
            listRequest.SearchType= CseResource.ListRequest.SearchTypeEnum.Image;

            Console.WriteLine("Start...");
            IList<Result> paging = new List<Result>();
            var count = 0;
            string bestImageURL = "";
            while (paging != null)
            {
                Console.WriteLine($"Page {count}");
                listRequest.Start = count * 10 + 1;
                listRequest.Num = 10;
                paging = listRequest.Execute().Items;
                if (paging != null)
                    foreach (var item in paging)
                        //most likely a Twitter image
                        if (item.Image.ContextLink.Contains("pbs.twimg.com")){
                            bestImageURL = item.Image.ContextLink;
                            break;
                        //most likely a LinkedIn image
                        } else if (item.Image.Height == 200 && item.Image.Width == 200 && item.Image.ContextLink.Contains("base64")) {
                            bestImageURL = item.Image.ContextLink;
                            break;
                        }

                count++;
            }
            Console.WriteLine("Done.");
            Console.ReadLine();
            return bestImageURL;
        }
    }
}

public interface IGoogleCustomSearchService
{
        string DoGoogleImageSearch(string searchQuery);
}


