using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Components.RatingControl
{
    public class EventTableViewComponent : ViewComponent
    {
        public EventTableViewComponent() { }

        public IViewComponentResult Invoke(List<PersonalIntranetBot.Models.PersonalIntranetBotMeetingViewModel> eventList)
        {
            return View("Default", eventList);
        }
    }
}