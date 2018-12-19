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

    }
}
