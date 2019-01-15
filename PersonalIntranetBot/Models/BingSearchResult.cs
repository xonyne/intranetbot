/* 
*  Author: Kevin Suter
*  Description: DTO (data transfer object) for Bing Web Search.
*  
*/
namespace PersonalIntranetBot.Models
{
    class BingSearchResult
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }
}
