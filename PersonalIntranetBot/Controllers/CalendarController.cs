/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalIntranetBot.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using PersonalIntranetBot.Models;
using System;
using PersonalIntranetBot.Services;

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
        // Load user's profile.
        // Get events in all the current user's mail folders.
        public async Task<IActionResult> Index()
        {
            List<OutlookEventsViewModel> items = new List<OutlookEventsViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                var identifier = User.FindFirst(Startup.ObjectIdentifierType)?.Value;
                _graphClient = _graphSdkHelper.GetAuthenticatedClient(identifier);
                IUserEventsCollectionPage events = await _graphClient.Me.Events.Request().GetAsync();
                items = await _personalIntranetBotService.GetOutlookCalendarEvents(_graphClient);
            }
            return View(items);
        }
    }
}


