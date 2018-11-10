﻿/* 
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

        public CalendarController(IConfiguration configuration, IHostingEnvironment hostingEnvironment, IGraphSdkHelper graphSdkHelper)
        {
            _configuration = configuration;
            _env = hostingEnvironment;
            _graphSdkHelper = graphSdkHelper;
        }

        [AllowAnonymous]
        // Load user's profile.
        // Get events in all the current user's mail folders.
        public async Task<IActionResult> Calendar()
        {
            List<OutlookEventsViewModel> items = new List<OutlookEventsViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                // Get user's id for token cache.
                var identifier = User.FindFirst(Startup.ObjectIdentifierType)?.Value;

                // Initialize the GraphServiceClient.
                var graphClient = _graphSdkHelper.GetAuthenticatedClient(identifier);

                // Get events.
                IUserEventsCollectionPage events = await graphClient.Me.Events.Request().GetAsync();

                items = await PersonalIntranetBotService.GetOutlookCalendarEvents(graphClient);
            }
            return View(items);
        }
    }
}


