using Microsoft.Graph;
using PersonalIntranetBot.Models;
using System.Collections.Generic;

namespace PersonalIntranetBot.Interfaces
{
    public interface IPersonalIntranetBotService
    {
        List<PersonalIntranetBotMeetingViewModel> GetOutlookCalendarEvents(GraphServiceClient graphClient);
    }
}
