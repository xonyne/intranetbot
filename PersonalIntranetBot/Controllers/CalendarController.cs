/* 
*  Author: Kevin Suter
*  Description: This class is used to handle the page with list of all meetings. 
*  It triggers the loading of the calendar events from the Graph API.
*  
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using PersonalIntranetBot.Helpers;
using PersonalIntranetBot.Interfaces;
using PersonalIntranetBot.Models;
using PersonalIntranetBot.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly IGraphSdkHelper _graphSdkHelper;
        private GraphServiceClient _graphClient;
        private readonly IPersonalIntranetBotService _personalIntranetBotService;
        private readonly DBModelContext _context;


        public CalendarController(DBModelContext context, IConfiguration configuration, IHostingEnvironment hostingEnvironment, IGraphSdkHelper graphSdkHelper, IPersonalIntranetBotService personalIntranetBotService)
        {
            _configuration = configuration;
            _env = hostingEnvironment;
            _graphSdkHelper = graphSdkHelper;
            _context = context;
            _personalIntranetBotService = personalIntranetBotService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<PersonalIntranetBotMeetingViewModel> items = new List<PersonalIntranetBotMeetingViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                var identifier = User.FindFirst(Startup.ObjectIdentifierType)?.Value;
                _graphClient = _graphSdkHelper.GetAuthenticatedClient(identifier);
                IUserEventsCollectionPage events = await _graphClient.Me.Events.Request().GetAsync();
                items = _personalIntranetBotService.GetOutlookCalendarEvents(_graphClient);
            }
            return View(items);
        }

        public IPersonalIntranetBotService IPersonalIntranetBotService
        {
            get => default(IPersonalIntranetBotService);
            set
            {
            }
        }
    }
}


