/* 
*  Author: Kevin Suter
*  Description: This class contains the whole view model for a meeting.
*  
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalIntranetBot.Models
{
    // An outlook meeting object in the GUI
    public class PersonalIntranetBotMeetingViewModel
    {
        public string MeetingId { get; set; }
        public string Subject { get; set; }
        [DisplayFormat(DataFormatString = "{0:ddd, dd.MM.yy HH:mm}")]
        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:ddd, dd.MM.yy HH:mm}")]
        public DateTime End { get; set; }
        public Location Location { get; set; }
        public string GoogleMapsURL { get; set; }
        public List<Attendee> Attendees { get; set; }
        public PersonalIntranetBotMeetingContentViewModel MeetingContent { get; set; }
    }
}