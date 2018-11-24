using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PersonalIntranetBot.Models
{
    // An entity, such as a user, group, or message.
    public class OutlookEventsViewModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:ddd, dd.MM.yy HH:mm}")]
        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:ddd, dd.MM.yy HH:mm}")]
        public DateTime End { get; set; }
        public string Location { get; set; }
        //public string AttendeeEmailAddresses { get; set; }
        public List<string> AttendeeEmailAddresses { get; set; }
        public string GoogleMapsURL { get; set; }
        // Key: Name of person, Value: URL to LinkedIn profile (empty string if not found)
        public List<Attendee> Attendees { get; set; }
    }
}