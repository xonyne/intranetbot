/* 
*  Author: Kevin Suter
*  Description: This component class is used to render the table with the calendar events of the user.
*  
*/
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