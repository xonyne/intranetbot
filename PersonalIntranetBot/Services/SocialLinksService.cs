using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PersonalIntranetBot.Helpers;
using PersonalIntranetBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace PersonalIntranetBot.Services
{

    class BingSearchResult
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }

    public static class SocialLinksService
    {
        public enum LinkedInPublicProfileInformation { PROFILEIMAGEURL, CURRENTJOBTITLE, CURRENTJOBCOMPANY, EDUCATIONLOCATION };
        public static string PROFILEIMAGEURL_XPATH = "//div[@class='profile-picture']";
        /*public static string PROFILEIMAGEURL_XPATH = "//*[@id=\"topcard\"]/div[1]/div[1]/a/img";*/
        public static string CURRENTJOBTITLE_XPATH = "//*[@id=\"topcard\"]/div[1]/div[2]/div/p";
        public static string CURRENTJOBCOMPANY_XPATH = "//*[@id=\"topcard\"]/div[1]/div[2]/div/table/tbody/tr[1]/td/ol/li/span/a";
        public static string EDUCATIONLOCATION_XPATH = "//*[@id=\"topcard\"]/div[1]/div[2]/div/table/tbody/tr[3]/td/ol/li/a";

        public static string LINKEDIN_URL_STRING = "/in/".ToLower();
        public static string LINKEDIN_SEARCH_STRING = "linkedin";

        public static string XING_URL_STRING = "/profile/".ToLower();
        public static string XING_SEARCH_STRING = "xing";

        public static string TWITTER_URL_STRING = "twitter.com".ToLower();
        public static string TWITTER_SEARCH_STRING = "twitter";

        private static List<BingSearchResult> PerformBingWebSearch(string searchString) {
            Thread.Sleep(1000);
            BingWebSearchService bingService = new BingWebSearchService();
            string bingWebSearchJSON = bingService.DoBingWebSearch(searchString).JsonResult;
            return ConvertBingWebSearchJSONResultToBingSearchResultObjects(bingWebSearchJSON);
        }

        public static string GetLinkedInAccountURLFromNameAndCompany(string name, string company)
        {
            string searchString = String.Join(" ", new[] { name, company, LINKEDIN_SEARCH_STRING });
            List<BingSearchResult> linkedInSearchResults = PerformBingWebSearch(searchString);

            string bestMatch="-";              
            foreach (BingSearchResult item in linkedInSearchResults)
            {
                bool isLinkedInProfileURL = item.URL.ToLower().Contains(LINKEDIN_URL_STRING.ToLower());
                bool isNameInDescription = item.Description.ToLower().Contains(name.ToLower());
                bool isCompanyInDescription = item.Description.ToLower().Contains(company.ToLower());

                // find best match
                if (isNameInDescription && isCompanyInDescription && isLinkedInProfileURL)
                {
                    bestMatch = item.URL.ToLower();
                    break;
                }
                else if (isNameInDescription && isLinkedInProfileURL)
                {
                    bestMatch = item.URL.ToLower();
                    break;
                }
            }
            return bestMatch;
        }

        public static string GetTwitterAccountURLFromNameAndCompany(string name, string company)
        {
            string searchString = String.Join(" ", new[] { name, company, TWITTER_SEARCH_STRING });
            List<BingSearchResult> twitterSearchResults = PerformBingWebSearch(searchString);

            string bestMatch = "-";
            foreach (BingSearchResult item in twitterSearchResults)
            {
                bool isTwitterProfileURL = item.URL.ToLower().Contains(TWITTER_URL_STRING.ToLower());
                bool isNameInDescription = item.Description.ToLower().Contains(name.ToLower());
                bool isCompanyInDescription = item.Description.ToLower().Contains(company.ToLower());

                // find best match
                if (isNameInDescription && isCompanyInDescription && isTwitterProfileURL)
                {
                    bestMatch = item.URL.ToLower();
                    break;
                }
                else if (isNameInDescription && isTwitterProfileURL)
                {
                    bestMatch = item.URL.ToLower();
                }
            }
            return bestMatch;
        }

        public static string GetXingAccountURLFromNameAndCompany(string name, string company)
        {
            string searchString = String.Join(" ", new[] { name, company, XING_SEARCH_STRING });
            List<BingSearchResult> xingSearchResults = PerformBingWebSearch(searchString);

            string bestMatch = "-";
            foreach (BingSearchResult item in xingSearchResults)
            {
                bool isXingProfileURL = item.URL.ToLower().Contains(XING_URL_STRING.ToLower());
                bool isNameInDescription = item.Description.ToLower().Contains(name.ToLower());
                bool isCompanyInDescription = item.Description.ToLower().Contains(company.ToLower());
              
                // find best match
                if (isNameInDescription && isCompanyInDescription && isXingProfileURL)
                {
                    bestMatch = item.URL.ToLower();
                    break;
                }
                else if (isNameInDescription && isXingProfileURL)
                {
                    bestMatch = item.URL.ToLower();
                }
            }
            return bestMatch;
        }

        /* Does not work like expected --> can't fetch LinkedIn public profile information */
        public static string GetLinkedInProfileInformation(string linkedInPublicProfileURL, LinkedInPublicProfileInformation requiredInformationType)
        {

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(linkedInPublicProfileURL);
            HtmlNodeCollection htmlNodes;

            string linkedInRequiredInformation = "";
            switch (requiredInformationType)
            {
                case LinkedInPublicProfileInformation.PROFILEIMAGEURL:
                    htmlNodes = doc.DocumentNode.SelectNodes(SocialLinksService.PROFILEIMAGEURL_XPATH);
                    if (htmlNodes != null)
                    {
                        linkedInRequiredInformation = htmlNodes[0].Attributes["src"].Value;
                    }
                    break;
                case LinkedInPublicProfileInformation.CURRENTJOBCOMPANY:
                    htmlNodes = doc.DocumentNode.SelectNodes(SocialLinksService.CURRENTJOBCOMPANY_XPATH);
                    if (htmlNodes != null)
                    {
                        linkedInRequiredInformation = htmlNodes[0].InnerHtml;
                    }
                    break;
                case LinkedInPublicProfileInformation.CURRENTJOBTITLE:
                    htmlNodes = doc.DocumentNode.SelectNodes(SocialLinksService.CURRENTJOBTITLE_XPATH);
                    if (htmlNodes != null)
                    {
                        linkedInRequiredInformation = htmlNodes[0].InnerHtml;
                    }
                    break;
                case LinkedInPublicProfileInformation.EDUCATIONLOCATION:
                    htmlNodes = doc.DocumentNode.SelectNodes(SocialLinksService.EDUCATIONLOCATION_XPATH);
                    if (htmlNodes != null)
                    {
                        linkedInRequiredInformation = htmlNodes[0].InnerHtml;
                    }
                    break;
            }


            return linkedInRequiredInformation;
        }

        /* Work in progress --> trying to fetch LinkedIn public profile information */
        public static string GetLinkedInProfileInformationChromHeadless(string linkedInPublicProfileURL, LinkedInPublicProfileInformation requiredInformationType)
        {
            /*string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);*/

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");

            string linkedInRequiredInformation = "";
            var browser = new ChromeDriver(@"C:\Users\gast.xonyne-PC\Source\Repos\intranetbot\PersonalIntranetBot\wwwroot\lib", chromeOptions);
            //Navigate to google page
            browser.Navigate().GoToUrl(linkedInPublicProfileURL);

            WebDriverWait waitForUsername = new WebDriverWait(browser, new System.TimeSpan(50000));
            waitForUsername.Until(b => browser.FindElement(By.XPath(SocialLinksService.PROFILEIMAGEURL_XPATH)));

            //Find the Search text box UI Element
            IWebElement element = browser.FindElement(By.XPath(SocialLinksService.PROFILEIMAGEURL_XPATH));

            return linkedInRequiredInformation;
        }

        private static List<BingSearchResult> ConvertBingWebSearchJSONResultToBingSearchResultObjects(string json)
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

    }
}
