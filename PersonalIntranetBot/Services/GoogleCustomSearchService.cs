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
using PersonalIntranetBot.Services;

namespace PersonalIntranetBot.Services
{
    public enum ImageType
    {
        COMPANY,
        XING,
        TWITTER,
        RECTANGULAR
    }

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
        public string DoGoogleImageSearch(string name, string company, ImageType imageType)
        {
            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = _apiKey });
            string searchString = name;
            switch (imageType)
            {
                case ImageType.COMPANY: searchString += " " + company; break;
                case ImageType.XING: searchString += " xing"; break;
                case ImageType.TWITTER: searchString += " twitter"; break;
                case ImageType.RECTANGULAR: break;
            }

            var listRequest = customSearchService.Cse.List(searchString);
            listRequest.Cx = _searchEngineId;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;

            Console.WriteLine("Start...");
            IList<Result> paging = new List<Result>();

            string emptyImageUrl = "";
            var count = 0;
            Console.WriteLine("---- Google image search starts ----");
            // max. 100 results are returned for JSON API requests (see https://developers.google.com/custom-search/v1/cse/list, start parameter)
            Console.WriteLine("Searching for '" + searchString + "' (" + name + ", " + company + ")");
            while (count < 1)
            {
                listRequest.Start = count * 10 + 1;
                listRequest.Num = 10;
                paging = listRequest.Execute().Items;
                if (paging != null)
                    foreach (var item in paging)
                    {
                        Console.WriteLine(item.Link);
                        switch (imageType)
                        {
                            case ImageType.COMPANY:
                                if (IsCompanyImage(item, name, company))
                                    return item.Link;
                                break;
                            case ImageType.XING:
                                if (IsXingImage(item))
                                    return item.Link;
                                break;
                            case ImageType.TWITTER:
                                if (IsTwitterImage(item))
                                    return item.Link;
                                break;
                            case ImageType.RECTANGULAR:
                                if (IsRectangularImage(item))
                                    return item.Link;
                                break;
                        }
                    }
                count++;
            }
            Console.WriteLine("---- Google image search ends ----");
            return emptyImageUrl;
        }

        private bool IsCompanyImage(Result item, string name, string company)
        {
            bool someLinkContainsCompany = (item.Link.Contains(company) || item.Image.ContextLink.Contains(company));
            bool imageURLContainsNamepart = false;
            string[] nameparts = name.Split(" ");
            foreach (string namepart in nameparts) {
                imageURLContainsNamepart = item.Link.Contains(namepart);
            }
            bool imageRectangular = item.Image.Height == item.Image.Width;

            return (someLinkContainsCompany && imageURLContainsNamepart && imageRectangular);
        }

        private bool IsXingImage(Result item)
        {
            bool imageURLContainsXing = item.Link.Contains("image");
            bool imageRectangular = item.Image.Height == item.Image.Width;

            return imageURLContainsXing && imageRectangular;
        }


        private bool IsTwitterImage(Result item)
        {
            bool imageURLContainsTwitter = item.Link.Contains("pbs.twimg.com");
            bool imageRectangular = item.Image.Height == item.Image.Width;
           
            return imageURLContainsTwitter && imageRectangular;
        }

        private bool IsRectangularImage(Result item)
        {
            bool imageRectangular = item.Image.Height == item.Image.Width;
            return imageRectangular;
        }

    }

}

public interface IGoogleCustomSearchService
{
    string DoGoogleImageSearch(string name, string company, ImageType imageType);
}


