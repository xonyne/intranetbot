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

    class LinkedInSearchResult
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }

    public class LinkedInService
    {
        public enum LinkedInPublicProfileInformation { PROFILEIMAGEURL, CURRENTJOBTITLE, CURRENTJOBCOMPANY, EDUCATIONLOCATION };
        public static string PROFILEIMAGEURL_XPATH = "//div[@class='profile-picture']";
        /*public static string PROFILEIMAGEURL_XPATH = "//*[@id=\"topcard\"]/div[1]/div[1]/a/img";*/
        public static string CURRENTJOBTITLE_XPATH = "//*[@id=\"topcard\"]/div[1]/div[2]/div/p";
        public static string CURRENTJOBCOMPANY_XPATH = "//*[@id=\"topcard\"]/div[1]/div[2]/div/table/tbody/tr[1]/td/ol/li/span/a";
        public static string EDUCATIONLOCATION_XPATH = "//*[@id=\"topcard\"]/div[1]/div[2]/div/table/tbody/tr[3]/td/ol/li/a";


        public static string LINKEDIN_SEARCH_SUFFIX = "linkedin";

        public static string GetLinkedInProfileURLFromNameAndCompany(string name, string company)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // add linked
            BingWebSearchService bingService = new BingWebSearchService();
            string searchString = String.Join(" ", new[] { name, company, LINKEDIN_SEARCH_SUFFIX });
            string bingWebSearchJSON = bingService.DoBingWebSearch(searchString).JsonResult;
            List<LinkedInSearchResult> linkedInSearchResults = ConvertBingSearchResultToLinkedInSearchResult(bingWebSearchJSON);
            string bestMatch="";
            string LINKEDIN_URL_SEARCH_STRING = "/in/".ToLower();
            
            foreach (LinkedInSearchResult item in linkedInSearchResults)
            {
                bool isLinkedInProfilePage = item.URL.ToLower().Contains(LINKEDIN_URL_SEARCH_STRING.ToLower());
                bool isNameInDescription = item.Description.ToLower().Contains(name.ToLower());
                bool isCompanyInDescription = item.Description.ToLower().Contains(company.ToLower());
                // find first match
                if (String.IsNullOrEmpty(bestMatch) && isLinkedInProfilePage) {
                    bestMatch = item.URL.ToLower();
                }
                // find best match
                if (isNameInDescription && isCompanyInDescription && isLinkedInProfilePage)
                {
                    bestMatch = item.URL.ToLower();
                }
            }
            return bestMatch;
        }

        private static List<LinkedInSearchResult> ConvertBingSearchResultToLinkedInSearchResult(string json)
        {
            List<LinkedInSearchResult> results = new List<LinkedInSearchResult>();
            JObject jObject = JObject.Parse(json);
            JToken jRoot = jObject["webPages"];
            JToken[] values = jRoot["value"].ToArray();
            foreach (JToken value in values)
            {
                results.Add(new LinkedInSearchResult
                {
                    Id = (string)value["id"],
                    Description = (string)value["name"],
                    URL = (string)value["url"],
                });
            }
            return results;
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
                    htmlNodes = doc.DocumentNode.SelectNodes(LinkedInService.PROFILEIMAGEURL_XPATH);
                    if (htmlNodes != null)
                    {
                        linkedInRequiredInformation = htmlNodes[0].Attributes["src"].Value;
                    }
                    break;
                case LinkedInPublicProfileInformation.CURRENTJOBCOMPANY:
                    htmlNodes = doc.DocumentNode.SelectNodes(LinkedInService.CURRENTJOBCOMPANY_XPATH);
                    if (htmlNodes != null)
                    {
                        linkedInRequiredInformation = htmlNodes[0].InnerHtml;
                    }
                    break;
                case LinkedInPublicProfileInformation.CURRENTJOBTITLE:
                    htmlNodes = doc.DocumentNode.SelectNodes(LinkedInService.CURRENTJOBTITLE_XPATH);
                    if (htmlNodes != null)
                    {
                        linkedInRequiredInformation = htmlNodes[0].InnerHtml;
                    }
                    break;
                case LinkedInPublicProfileInformation.EDUCATIONLOCATION:
                    htmlNodes = doc.DocumentNode.SelectNodes(LinkedInService.EDUCATIONLOCATION_XPATH);
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
            waitForUsername.Until(b => browser.FindElement(By.XPath(LinkedInService.PROFILEIMAGEURL_XPATH)));

            //Find the Search text box UI Element
            IWebElement element = browser.FindElement(By.XPath(LinkedInService.PROFILEIMAGEURL_XPATH));

            return linkedInRequiredInformation;
        }

    }
}
