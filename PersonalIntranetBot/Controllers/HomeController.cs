/* 
*  Author: Kevin Suter
*  Description: This class is used to render the home page and to handle user input on this page.
*  
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using PersonalIntranetBot.Helpers;
using PersonalIntranetBot.Interfaces;
using PersonalIntranetBot.Services;
using System.Threading.Tasks;

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

                ViewData["Response"] = await _graphService.GetGraphUserJson(graphClient, email, HttpContext);

                ViewData["Picture"] = await _graphService.GetGraphPictureBase64(graphClient, email, HttpContext);
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
                await _graphService.SendGraphEmail(graphClient, _env, recipients, HttpContext, "Test email sent from " + PersonalIntranetBotService.APPLICATON_NAME + ".", "Test", Request.Path);
                
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

        public IGraphService IGraphService
        {
            get => default(IGraphService);
            set
            {
            }
        }

    }
}
