/* 
*  Author: Kevin Suter
*  Description: This class is used to call the Google Custom Search API.
*  
*/
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using PersonalIntranetBot.Extensions;
using PersonalIntranetBot.Interfaces;
using System;
using System.Collections.Generic;

namespace PersonalIntranetBot.Services
{

    public class GoogleCustomSearchService : IGoogleCustomSearchService
    {
        private readonly string _apiKey;
        private readonly string _searchEngineId;

        public GoogleCustomSearchService(IConfiguration configuration)
        {
            var googleOptions = new GoogleCustomSearchOptions();
            configuration.Bind("GoogleCustomSearchConfig", googleOptions);
            _apiKey = googleOptions.AccessKey;
            _searchEngineId = googleOptions.SearchEngineId;
        }

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
            // max. 100 results are returned for JSON API requests (see https://developers.google.com/custom-search/v1/cse/list, 'start' parameter)
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

            return (someLinkContainsCompany && imageRectangular);
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

        public GoogleCustomSearchOptions GoogleCustomSearchOptions
        {
            get => default(GoogleCustomSearchOptions);
            set
            {
            }
        }

    }

}


