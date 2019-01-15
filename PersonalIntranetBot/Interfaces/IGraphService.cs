using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Interfaces
{
    public interface IGraphService
    {
        List<Event> GetGraphCalendarEvents(GraphServiceClient graphClient);
        Task<string> GetGraphUserJson(GraphServiceClient graphClient, string email, HttpContext httpContext);
        Task<string> GetGraphPictureBase64(GraphServiceClient graphClient, string email, HttpContext httpContext);
        Task SendGraphEmail(GraphServiceClient graphClient, IHostingEnvironment hostingEnvironment, string recipients, HttpContext httpContext, string comment, string meetingTitle, string name);
    }
}
