using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using PersonalIntranetBot.Helpers;
using PersonalIntranetBot.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Location = PersonalIntranetBot.Models.Location;

namespace PersonalIntranetBot.Services
{
    public class PersonalIntranetBotService : IPersonalIntranetBotService
    {
        private readonly DBModelContext _dbContext;
        private readonly IGoogleMapsService _googleMapsService;
        private readonly IGoogleCustomSearchService _googleCustomSearchService;
        private readonly ISocialLinkService _socialLinksService;
        private readonly IGraphService _graphService;
        private static string _personalIntranetBotName = "Personal Intranet Bot";

        public PersonalIntranetBotService(DBModelContext dbContext, IGoogleMapsService googleMapsService, ISocialLinkService socialLinksService, IGoogleCustomSearchService googleCustomSearchService, IGraphService graphService) {
            _dbContext = dbContext;
            _graphService = graphService;
            _googleMapsService = googleMapsService;
            _socialLinksService = socialLinksService;
            _googleCustomSearchService = googleCustomSearchService;
        }

        public async Task<List<PersonalIntranetBotMeetingViewModel>> GetOutlookCalendarEvents(GraphServiceClient graphClient)
        {
            List<PersonalIntranetBotMeetingViewModel> meetingViewItems = new List<PersonalIntranetBotMeetingViewModel>();
            IUserEventsCollectionPage meetingGraphItems = await _graphService.GetCalendarEvents(graphClient);
            if (meetingGraphItems?.Count > 0)
            {
                foreach (Event graphMeeting in meetingGraphItems)
                {
                    Location meetingLocation = GetAddressFromGraphLocation(graphMeeting.Location);
                    if (MeetingIsNotRecurringAndNotAllDay(graphMeeting) && MeetingIsNotInPast(graphMeeting)) {
                        string meetingIdWithNoSpecialChars = RemoveMeetingIdSpecialChars(graphMeeting.Id);
                        meetingViewItems.Add(new PersonalIntranetBotMeetingViewModel
                        {
                            MeetingId = meetingIdWithNoSpecialChars,
                            Subject = graphMeeting.Subject,
                            Start = DateTime.Parse(graphMeeting.Start.DateTime),
                            End = DateTime.Parse(graphMeeting.End.DateTime),
                            Location = meetingLocation,
                            GoogleMapsURL = _googleMapsService.GetGoogleMapsURL(meetingLocation.LocationString),
                            Attendees = GetAndUpdateMeetingAttendees(graphMeeting.Attendees),
                            MeetingContent = new PersonalIntranetBotMeetingContentViewModel {
                                MeetingId = meetingIdWithNoSpecialChars,
                                Subject = graphMeeting.Subject,
                                Description = graphMeeting.Body.Content,
                                Comments = LoadMeetingComments(meetingIdWithNoSpecialChars)
                            }
                        });
                    }
                }
            }
            // Order events by start date ascending
            meetingViewItems = meetingViewItems.OrderBy(e => e.Start).ToList();

            //Save attendees to database

            return meetingViewItems;
        }

        private string RemoveMeetingIdSpecialChars(string id) {
            return id.Replace("=", "").Replace("_", "").Trim();
        }

        private List<MeetingComment> LoadMeetingComments(string id)
        {
            Task<List<Models.MeetingComment>> meetingCommentsTask = _dbContext.MeetingComments.Where(s => s.MeetingId.Equals(id)).ToListAsync();
            meetingCommentsTask.Wait();
            return meetingCommentsTask.Result;
        }

        private List<Models.Attendee> GetAndUpdateMeetingAttendees(IEnumerable<Microsoft.Graph.Attendee> meetingAttendees) {
           Task<List<Models.Attendee>> dbAttendeesTask = _dbContext.Attendees.Include(s => s.SocialLinks).ToListAsync();
           dbAttendeesTask.Wait();
           List<Models.Attendee> dbAttendees = dbAttendeesTask.Result;

            List<Models.Attendee> results = new List<Models.Attendee>();
            foreach (Microsoft.Graph.Attendee graphAttendee in meetingAttendees) {
                string meetingAttendeeEmailAddress = graphAttendee.EmailAddress.Address.ToString();
                if (meetingAttendeeEmailAddress != null)
                {
                    if (!dbAttendees.Any(dbAttendee => dbAttendee.EmailAddress.ToLower() == meetingAttendeeEmailAddress.ToLower()))
                    {
                        var attendee = new Models.Attendee
                        {
                            EmailAddress = meetingAttendeeEmailAddress,
                            DisplayName = ToTitleCase(GetNameFromEMailAddress(meetingAttendeeEmailAddress)),
                            IsPerson = true,
                            SocialLinks = GetSocialLinksForEmailAddress(meetingAttendeeEmailAddress),
                            ImageURL = _googleCustomSearchService.DoGoogleImageSearch(GetNameFromEMailAddress(meetingAttendeeEmailAddress), GetCompanyFromEMailAddress(meetingAttendeeEmailAddress)),
                            CurrentJobTitle = "",
                            CurrentJobCompany = ToTitleCase(GetCompanyFromEMailAddress(meetingAttendeeEmailAddress)),
                            EducationLocation = "",
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = _personalIntranetBotName
                        };
                        results.Add(attendee);
                        _dbContext.Add(attendee);
                    }
                    else {
                        results.Add(dbAttendees.First(item => item.EmailAddress.ToLower() == meetingAttendeeEmailAddress.ToLower()));
                    }
                    
                }

            }
            _dbContext.SaveChanges();

            return results;
        }

        private string GetNameFromEMailAddress(string emailAddress) {
            
            // get first part of email address and replace . by space (split first and last name)
            string name = emailAddress.Split("@")[0].Replace(".", " ").Trim();
            
            // set Umlauts
            name = name.Replace("ue", "ü");
            name = name.Replace("ae", "ä");
            name = name.Replace("oe", "ö");

            return name;
        }

        private string GetCompanyFromEMailAddress(string emailAddress)
        {
            // get second part of email address and get only company name
            return emailAddress.Split("@")[1].Split(".")[0];
        }


        private bool MeetingIsNotRecurringAndNotAllDay(Event meeting)
        {
            return (!(bool)meeting.IsAllDay && meeting.Recurrence == null);
        }

        private bool MeetingIsNotInPast(Event meeting)
        {
            return (DateTime.Parse(meeting.End.DateTime) >= System.DateTime.Now);
        }

        private List<SocialLink> GetSocialLinksForEmailAddress(String emailAddress)
        {
            List<SocialLink> results = new List<SocialLink>
            {
                new SocialLink
                {
                    Type = SocialLink.LinkType.LINKEDIN,
                    URL = _socialLinksService.GetLinkedInAccountURLFromNameAndCompany(GetNameFromEMailAddress(emailAddress), GetCompanyFromEMailAddress(emailAddress))
                },
                new SocialLink
                {
                    Type = SocialLink.LinkType.XING,
                    URL = _socialLinksService.GetXingAccountURLFromNameAndCompany(GetNameFromEMailAddress(emailAddress), GetCompanyFromEMailAddress(emailAddress))
                },
                new SocialLink
                {
                    Type = SocialLink.LinkType.TWITTER,
                    URL = _socialLinksService.GetTwitterAccountURLFromNameAndCompany(GetNameFromEMailAddress(emailAddress), GetCompanyFromEMailAddress(emailAddress))
                }
            };
            return results;

        }

        private String GetAttendeeEmailAddressesAsString(IEnumerable<Microsoft.Graph.Attendee> collection, String separator)
        {
            using (var enumerator = collection.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return String.Empty;
                }

                var builder = new StringBuilder().Append(enumerator.Current.EmailAddress.Address);

                while (enumerator.MoveNext())
                {
                    builder.Append(separator).Append(enumerator.Current.EmailAddress.Address);
                }

                return builder.ToString();
            }
        }

        private List<string> GetAttendeeEmailAddressesAsList(IEnumerable<Microsoft.Graph.Attendee> collection)
        {
            List<string> result = new List<string>();
            using (var enumerator = collection.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return result;
                }

                result.Add(enumerator.Current.EmailAddress.Address);
                while (enumerator.MoveNext())
                {
                    result.Add(enumerator.Current.EmailAddress.Address);
                }
            }
            return result;
        }

        private PersonalIntranetBot.Models.Location GetAddressFromGraphLocation(Microsoft.Graph.Location location)
        {
            string meetingLocation="";
            if (location.Address != null)
            {
                String street = String.IsNullOrEmpty(location.Address.Street) ? "" : location.Address.Street;
                String postalCode = String.IsNullOrEmpty(location.Address.PostalCode) ? "" : ", " + location.Address.PostalCode + " ";
                String city = String.IsNullOrEmpty(location.Address.City) ? "" : location.Address.City;
                meetingLocation= street + postalCode + city;
            } 
            if (location.DisplayName != null) {
                meetingLocation= location.DisplayName;
}
            return new PersonalIntranetBot.Models.Location
            {
                LocationString = meetingLocation,
                IsAddress = CheckMeetingLocationIsAddress(meetingLocation),
            };
        }

        private bool CheckMeetingLocationIsAddress(string meetingLocation)
        {
            return meetingLocation.Contains(",") && meetingLocation.Any(char.IsDigit);
        }

        public string ToTitleCase(string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title);
        }

    }

    public interface IPersonalIntranetBotService
    {
        Task<List<PersonalIntranetBotMeetingViewModel>> GetOutlookCalendarEvents(GraphServiceClient graphClient);
    }
}
