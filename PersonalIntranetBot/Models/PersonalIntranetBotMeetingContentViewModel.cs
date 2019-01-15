/* 
*  Author: Kevin Suter
*  Description: This class contains the view model for only the content of a meeting (subject, description, comments).
*  Subject and Description are fetched from the Graph API, Comments are stored within this application.
*  
*/
using System.Collections.Generic;

namespace PersonalIntranetBot.Models
{

    public class PersonalIntranetBotMeetingContentViewModel
    {
        public string MeetingId { get; set; }
        public string Subject { get; internal set; }
        public string Description { get; set; }
        public List<MeetingComment> Comments { get; set; }
    }
}
