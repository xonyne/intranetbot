/* 
*  Author: Kevin Suter
*  Description: This class is used to handle all user actions regarding meeting attendees.
*  
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalIntranetBot.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Controllers
{
    public class AttendeeController : Controller
    {
        private readonly DBModelContext _context;

        public AttendeeController(DBModelContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SaveSocialLink(SocialLink incomingSocialLink)
        {
            SocialLink socialLink = new SocialLink();
            Attendee affectedAttendee = new Attendee();
            if (ModelState.IsValid)
            {
                try
                {
                    affectedAttendee = _context.Attendees.Single(x => x.AttendeeId == incomingSocialLink.AttendeeId);
                    affectedAttendee.LastUpdated = DateTime.Now;
                    if (User.Identity.IsAuthenticated)
                    {
                        affectedAttendee.LastUpdatedBy = User.Identity.Name;
                    }
                    else {
                        affectedAttendee.LastUpdatedBy = "anonymous";

                    }

                    socialLink = _context.SocialLinks.Single(x => x.SocialLinkId == incomingSocialLink.SocialLinkId);
                    socialLink.URL = incomingSocialLink.URL;
                    _context.Update(socialLink);
                    _context.Update(affectedAttendee);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialLinkExists(incomingSocialLink.SocialLinkId))
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.BadGateway;
                        return Content("Error: Could not find social link in database");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Content("Last updated: " + affectedAttendee.LastUpdated + " by " + affectedAttendee.LastUpdatedBy);
        }

        [HttpPost]
        public async Task<IActionResult> SaveImageURL(Attendee incomingAttendee)
        {
            Attendee affectedAttendee = new Attendee();
            if (ModelState.IsValid)
            {
                try
                {
                    affectedAttendee = _context.Attendees.Single(x => x.AttendeeId == incomingAttendee.AttendeeId);
                    affectedAttendee.LastUpdated = DateTime.Now;
                    if (User.Identity.IsAuthenticated)
                    {
                        affectedAttendee.LastUpdatedBy = User.Identity.Name;
                    }
                    else
                    {
                        affectedAttendee.LastUpdatedBy = "anonymous";

                    }
                    affectedAttendee.ImageURL = incomingAttendee.ImageURL;
                    _context.Update(affectedAttendee);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendeeExists(affectedAttendee.AttendeeId))
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.BadGateway;
                        return Content("Error: Could not find attendee in database");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Content("Last updated: " + affectedAttendee.LastUpdated + " by " + affectedAttendee.LastUpdatedBy);
        }

        private bool SocialLinkExists(int id)
        {
            return _context.SocialLinks.Any(e => e.SocialLinkId == id);
        }

        private bool AttendeeExists(int id)
        {
            return _context.Attendees.Any(e => e.AttendeeId == id);
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