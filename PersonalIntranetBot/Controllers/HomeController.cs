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
using PersonalIntranetBot.Services;
using Microsoft.AspNetCore.Http;

namespace PersonalIntranetBot.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly IGraphSdkHelper _graphSdkHelper;
        private readonly IGraphService _graphService;

        public HomeController(IConfiguration configuration, IHostingEnvironment hostingEnvironment, IGraphSdkHelper graphSdkHelper, IGraphService graphService)
        {
            _configuration = configuration;
            _env = hostingEnvironment;
            _graphSdkHelper = graphSdkHelper;
            _graphService = graphService;
        }

        [AllowAnonymous]
        // Load user's profile.
        public async Task<IActionResult> Index(string email)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Get users's email.
                email = email ?? User.FindFirst("preferred_username")?.Value;
                ViewData["Email"] = email;

                // Get user's id for token cache.
                var identifier = User.FindFirst(Startup.ObjectIdentifierType)?.Value;

                // Initialize the GraphServiceClient.
                var graphClient = _graphSdkHelper.GetAuthenticatedClient(identifier);

                ViewData["Response"] = await _graphService.GetUserJson(graphClient, email, HttpContext);

                ViewData["Picture"] = await _graphService.GetPictureBase64(graphClient, email, HttpContext);
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        // Send an email message from the current user.
        public async Task<IActionResult> SendEmail(string recipients)
        {
            if (string.IsNullOrEmpty(recipients))
            {
                TempData["Message"] = "Please add a valid email address to the recipients list!";
                return RedirectToAction("Index");
            }

            try
            {
                // Get user's id for token cache.
                var identifier = User.FindFirst(Startup.ObjectIdentifierType)?.Value;

                // Initialize the GraphServiceClient.
                var graphClient = _graphSdkHelper.GetAuthenticatedClient(identifier);

                // Send the email.
                await _graphService.SendEmail(graphClient, _env, recipients, HttpContext, "Mein Kommentar", "Neues Meeting", Request.Path);
                
                // Reset the current user's email address and the status to display when the page reloads.
                TempData["Message"] = "Success! Your mail was sent.";
                return RedirectToAction("Index");
            }
            catch (ServiceException se)
            {
                if (se.Error.Code == "Caller needs to authenticate.") return new EmptyResult();
                return RedirectToAction("Error", "Home", new { message = "Error: " + se.Error.Message });
            }
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
