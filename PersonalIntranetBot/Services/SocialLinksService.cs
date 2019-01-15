/* 
*  Author: Kevin Suter
*  Description: This class is used to obtain the social media links for the meeting attendees. 
*  It uses the BingWebSearchService to fullfil that task.
*  
*/
using Newtonsoft.Json.Linq;
using PersonalIntranetBot.Interfaces;
using PersonalIntranetBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PersonalIntranetBot.Services
{
    public class SocialLinksService : ISocialLinksService
    {
        private readonly IBingWebSearchService _bingWebSearchService;

        public static readonly string NO_SOCIAL_LINK ="-";
        public enum LinkedInPublicProfileInformation { PROFILEIMAGEURL, CURRENTJOBTITLE, CURRENTJOBCOMPANY, EDUCATIONLOCATION };

        private static readonly string LINKEDIN_URL_STRING = "/in/".ToLower();
        private static readonly string LINKEDIN_SEARCH_STRING = "linkedin";

        private static readonly string XING_URL_STRING = "/profile/".ToLower();
        private static readonly string XING_SEARCH_STRING = "xing";

        private static readonly string TWITTER_URL_STRING = "twitter.com".ToLower();
        private static readonly string TWITTER_SEARCH_STRING = "twitter";

        public SocialLinksService (IBingWebSearchService bingWebSearchService) {
            _bingWebSearchService = bingWebSearchService;
        }

        private List<BingSearchResult> PerformBingWebSearch(string searchString) {      
            string bingWebSearchJSON = _bingWebSearchService.DoBingWebSearch(searchString).JsonResult;
            return ConvertBingWebSearchJSONResultToBingSearchResultObjects(bingWebSearchJSON);
        }

        public string GetLinkedInAccountURLFromNameAndCompany(string name, string company)
        {
            return GetSocialLinkURL(name, company, LINKEDIN_SEARCH_STRING, LINKEDIN_URL_STRING);
        }

        public string GetTwitterAccountURLFromNameAndCompany(string name, string company)
        {
            return GetSocialLinkURL(name, company, TWITTER_SEARCH_STRING, TWITTER_URL_STRING);
        }

        public string GetXingAccountURLFromNameAndCompany(string name, string company)
        {
            return GetSocialLinkURL(name, company, XING_SEARCH_STRING, XING_URL_STRING);
        }

        private string GetSocialLinkURL(string name, string company, string searchSuffix, string urlMatchingString) {
            string searchString = String.Join(" ", new[] { name, company, searchSuffix });
            List<BingSearchResult> bingSearchResults = PerformBingWebSearch(searchString);

            string bestMatch = NO_SOCIAL_LINK;
            foreach (BingSearchResult item in bingSearchResults)
            {
                bool isSocialProfileURL = item.URL.ToLower().Contains(urlMatchingString.ToLower());
                bool isNameInDescription = item.Description.ToLower().Contains(name.ToLower());
                bool isCompanyInDescription = item.Description.ToLower().Contains(company.ToLower());
               
                if (isNameInDescription && isCompanyInDescription && isSocialProfileURL)
                {
                    return item.URL.ToLower();
                }

                if (isNameInDescription && isSocialProfileURL)
                {
                    bestMatch = item.URL.ToLower();
                }
            }
            return bestMatch;
        }

        private List<BingSearchResult> ConvertBingWebSearchJSONResultToBingSearchResultObjects(string json)
        {
            List<BingSearchResult> results = new List<BingSearchResult>();
            JObject jObject = JObject.Parse(json);
            JToken jRoot = jObject["webPages"];
            JToken[] values = jRoot["value"].ToArray();
            foreach (JToken value in values)
            {
                results.Add(new BingSearchResult
                {
                    Id = (string)value["id"],
                    Description = (string)value["name"],
                    URL = (string)value["url"],
                });
            }
            return results;
        }

        public IBingWebSearchService IBingWebSearchService
        {
            get => default(IBingWebSearchService);
            set
            {
            }
        }
    }

}
