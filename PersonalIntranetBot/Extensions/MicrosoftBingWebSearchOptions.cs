/* 
*  Author: Kevin Suter
*  Description: This class stores the options for the Microsoft Bing Web Search API.
*  
*/
namespace PersonalIntranetBot.Extensions
{
    public class MicrosoftBingWebSearchOptions
    {
        public string AccessKey { get; set; }

        public string UriBase { get; set; }

        public int SearchDelay { get; set; }
    }
}
