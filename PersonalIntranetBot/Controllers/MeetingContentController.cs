/* 
*  Author: Kevin Suter
*  Description: This class is used to handle all actions regarding the meeting content (e.g. meeting comments).
*  
*/
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using PersonalIntranetBot.Helpers;
using PersonalIntranetBot.Interfaces;
using PersonalIntranetBot.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Controllers
{
    public class MeetingContentController : Controller
    {
        private readonly DBModelContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IGraphSdkHelper _graphSdkHelper;
        private GraphServiceClient _graphClient;
        private readonly IGraphService _graphService;

        public MeetingContentController(DBModelContext context, IGraphSdkHelper graphSdkHelper, IGraphService graphService, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _env = hostingEnvironment;
            _graphSdkHelper = graphSdkHelper;
            _graphService = graphService;
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
                    else
                    {
                        _context.Update(meetingComment);
                    }

                    await _context.SaveChangesAsync();

                    if (User.Identity.IsAuthenticated)
                    {
                        var identifier = User.FindFirst(Startup.ObjectIdentifierType)?.Value;
                        _graphClient = _graphSdkHelper.GetAuthenticatedClient(identifier);
                        // Remove author and external attendees from recipients (attendees with different domain)
                        //incomingMeetingComment.NotificationRecipients;
                        await _graphService.SendGraphEmail(_graphClient, _env, "", HttpContext, incomingMeetingComment.Comment, "New comment: ", User.Identity.Name);
                    }

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

        public DBModelContext DBModelContext
        {
            get => default(DBModelContext);
            set
            {
            }
        }
    }
}