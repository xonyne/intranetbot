using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public Location Location { get; set; }
        public List<string> AttendeeEmailAddresses { get; set; }
        public string GoogleMapsURL { get; set; }
        public List<Attendee> Attendees { get; set; }
    }
}