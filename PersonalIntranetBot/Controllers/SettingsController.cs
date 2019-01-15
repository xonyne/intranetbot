/* 
*  Author: Kevin Suter
*  Description: This class is used to handle the settings page in the application.
*  
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalIntranetBot.Models;
using Attendee = PersonalIntranetBot.Models.Attendee;

namespace PersonalIntranetBot.Controllers
{
    public class SettingsController : Controller
    {
        private readonly DBModelContext _context;

        public SettingsController(DBModelContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteAllDatabaseContent()
        {
            var attendees = _context.Set<Attendee>();
            _context.Attendees.RemoveRange(attendees);

            var socialLinks = _context.Set<SocialLink>();
            _context.SocialLinks.RemoveRange(socialLinks);

            var comments = _context.Set<MeetingComment>();
            _context.MeetingComments.RemoveRange(comments);

            _context.SaveChanges();

            return Content("All database content successfully deleted!");
        }

        public DBModelContext DBModelContext
        {
            get => default(DBModelContext);
            set
            {
            }
        }

    }
}
