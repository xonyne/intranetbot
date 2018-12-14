using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
