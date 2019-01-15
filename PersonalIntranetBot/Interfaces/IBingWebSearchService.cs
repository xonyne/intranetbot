using PersonalIntranetBot.Models;

namespace PersonalIntranetBot.Interfaces
{
    public interface IBingWebSearchService
    {
        BingJSONResult DoBingWebSearch(string searchQuery);
    }
}
