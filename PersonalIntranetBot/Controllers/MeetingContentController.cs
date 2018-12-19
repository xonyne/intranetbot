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
        public async Task<JsonResult> SaveMeetingComment(MeetingComment incomingMeetingComment)
        {
            MeetingComment meetingComment = new MeetingComment();
            if (ModelState.IsValid)
            {
                try
                {
                    meetingComment = incomingMeetingComment;
                    meetingComment.LastUpdated = DateTime.Now;
                    meetingComment.LastUpdatedBy = User.Identity.IsAuthenticated == true ? User.Identity.Name : "anonymous";

                    if (meetingComment == null)
                    {
                        _context.Add(meetingComment);
                    }
                    else {
                        _context.Update(meetingComment);
                    }
                    
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadGateway;
                    return Json("Error: Could not save meeting comment in database");
                }
            }

            return Json(meetingComment);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteMeetingComment(MeetingComment incomingMeetingComment)
        {
            MeetingComment meetingComment = new MeetingComment();
            if (ModelState.IsValid)
            {
                try
                {
                    meetingComment = _context.MeetingComments.Single(x => x.MeetingCommentId == incomingMeetingComment.MeetingCommentId);
                    _context.Remove(meetingComment);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadGateway;
                    return Json("Error: Could not delete meeting comment in database");
                }
            }

            return Json(meetingComment);
        }
    }
}