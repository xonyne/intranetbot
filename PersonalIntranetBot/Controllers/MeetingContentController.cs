using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalIntranetBot.Models;

namespace PersonalIntranetBot.Controllers
{
    public class MeetingContentController : Controller
    {
        private readonly DBModelContext _context;

        public MeetingContentController(DBModelContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SaveMeetingComment(MeetingComment incomingMeetingComment)
        {
            MeetingComment meetingComment = new MeetingComment();

            if (User.Identity.IsAuthenticated)
            {
                meetingComment.LastUpdatedBy = User.Identity.Name;
            }
            else
            {
                meetingComment.LastUpdatedBy = "anonymous";

            }

            if (ModelState.IsValid)
            {
                try
                {
                    meetingComment.Comment = incomingMeetingComment.Comment;
                    meetingComment.LastUpdated = DateTime.Now;


                    _context.Add(meetingComment);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadGateway;
                    return Content("Error: Could not save meeting comment in database");
                }
            }

            return Content("Last updated: " + meetingComment.LastUpdated + " by " + meetingComment.LastUpdatedBy);
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
    }
}