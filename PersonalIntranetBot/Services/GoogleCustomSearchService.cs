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
        public string DoGoogleImageSearch(string name, string company)
        {
            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = _apiKey });
            var listRequest = customSearchService.Cse.List(name + " " + company);
            listRequest.Cx = _searchEngineId;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;

            Console.WriteLine("Start...");
            IList<Result> paging = new List<Result>();

            string bestImageURL = "";
            bool twitterImageFound = false;
            bool linkedInImageFound = false;
            bool companyImageFound = false;
            bool rectangularImageFound = false;
            var count = 0;
            Console.WriteLine("---- Google image search starts ----");
            // max. 100 results are return for JSON API requests (see https://developers.google.com/custom-search/v1/cse/list, start parameter)
            while (count < 10)
            {
                Console.WriteLine($"Page {count}");
                listRequest.Start = count * 10 + 1;
                listRequest.Num = 10;
                paging = listRequest.Execute().Items;
                if (paging != null)
                    foreach (var item in paging)
                    {
                        // it's an image of the company!
                        if (!companyImageFound && IsCompanyImage(item, company))
                        {
                            Console.WriteLine("Company image found: " + item.Link + "  (" + name + ") ");
                            companyImageFound = true;
                            bestImageURL = item.Link;
                        }
                        //most likely a LinkedIn image
                        else if (!linkedInImageFound && !twitterImageFound && !companyImageFound && IsLinkedInImage(item))
                        {
                            Console.WriteLine("LinkedIn image found: " + item.Link + "  (" + name + ") ");
                            linkedInImageFound = true;
                            bestImageURL = item.Link;
                        }
                        //most likely a Twitter image
                        else if (!twitterImageFound && !linkedInImageFound && !companyImageFound && IsTwitterImage(item))
                        {
                            Console.WriteLine("Twitter image found: " + item.Link + "  (" + name + ") ");
                            twitterImageFound = true;
                            bestImageURL = item.Link;
                        }
                        // height to width ratio are equal
                        else if (!linkedInImageFound && !twitterImageFound && !companyImageFound && !rectangularImageFound && IsRectangularImage(item))
                        {
                            Console.WriteLine("Rectangular image found: " + item.Link + "  (" + name + ") ");
                            rectangularImageFound = true;
                            bestImageURL = item.Link;
                        }
                        Console.WriteLine(name + ": " + item.Image.ContextLink + " (Context Link) | " + item.Link + " (Image Link)");
                    }
                count++;
            }
            Console.WriteLine("---- Google image search ends ----");
            return bestImageURL;
        }

        private bool IsCompanyImage(Result item, string company)
        {
            return (item.Link.Contains(company) || item.Image.ContextLink.Contains(company)) && item.Image.Height == item.Image.Width;
        }

        private bool IsLinkedInImage(Result item)
        {
            return item.Link.Contains("linkedin") && item.Image.Height == item.Image.Width;
        }


        private bool IsTwitterImage(Result item)
        {
            return item.Link.Contains("pbs.twimg.com") && item.Image.Height == item.Image.Width;
        }

        private bool IsRectangularImage(Result item)
        {
            return item.Image.Height == item.Image.Width;
        }

    }

}



public interface IGoogleCustomSearchService
{
    string DoGoogleImageSearch(string name, string company);
}


