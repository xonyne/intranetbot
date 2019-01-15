/* 
*  Author: Kevin Suter
*  Description: This class contains the main functionality for enriching calendar events with additional information. 
*  It uses the other services of the application accessing external systems for obtaining the information needed.
*  
*/
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using PersonalIntranetBot.Interfaces;
using PersonalIntranetBot.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location = PersonalIntranetBot.Models.Location;

namespace PersonalIntranetBot.Services
{
    public class PersonalIntranetBotService : IPersonalIntranetBotService
    {
        private readonly DBModelContext _dbContext;
        private readonly IGoogleMapsService _googleMapsService;
        private readonly IGoogleCustomSearchService _googleCustomSearchService;
        private readonly ISocialLinksService _socialLinksService;
        private readonly IGraphService _graphService;
        public static readonly string APPLICATON_NAME = "Personal Intranet Bot";
        public static readonly string JOB_NOT_FOUND = "Not found";
        public static readonly string EMPTY_IMG_URL = "/images/blank-profile-picture.svg";

        public PersonalIntranetBotService(DBModelContext dbContext, IGoogleMapsService googleMapsService, ISocialLinksService socialLinksService, IGoogleCustomSearchService googleCustomSearchService, IGraphService graphService) {
            _dbContext = dbContext;
            _graphService = graphService;
            _googleMapsService = googleMapsService;
            _socialLinksService = socialLinksService;
            _googleCustomSearchService = googleCustomSearchService;
        }

        public List<PersonalIntranetBotMeetingViewModel> GetOutlookCalendarEvents(GraphServiceClient graphClient)
        {
            List<PersonalIntranetBotMeetingViewModel> meetingViewItems = new List<PersonalIntranetBotMeetingViewModel>();
            List<Event> meetingGraphItems = _graphService.GetGraphCalendarEvents(graphClient);
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
                        List<SocialLink> socialLinks = GetSocialLinksForEmailAddress(meetingAttendeeEmailAddress);
                        string imageURL = GetImageURL(meetingAttendeeEmailAddress, socialLinks);
                        var attendee = new Models.Attendee
                        {
                            EmailAddress = meetingAttendeeEmailAddress,
                            DisplayName = ToTitleCase(GetNameFromEMailAddress(meetingAttendeeEmailAddress)),
                            IsPerson = true,
                            SocialLinks = socialLinks,
                            ImageURL = imageURL,
                            CurrentJobTitle = GetJobTitleForAttendee(socialLinks),
                            CurrentJobCompany = GetCompanyWebsiteFromEmailAddress(meetingAttendeeEmailAddress),
                            EducationLocation = "",
                            LastUpdated = DateTime.Now,
                            LastUpdatedBy = APPLICATON_NAME
                        };
                        results.Add(attendee);
                        _dbContext.Add(attendee);
                    }
                    else {
                        results.Add(dbAttendees.First(item => item.EmailAddress.ToLower() == meetingAttendeeEmailAddress.ToLower()));
                    }

                }

            }
            //Save attendees in database
            _dbContext.SaveChanges();

            return results;
        }


            // Try to get job title from Xing. If not possible, return 'Not found' string.
        private string GetJobTitleForAttendee(List<SocialLink> socialLinks)
            {
            string xingURL = socialLinks.Find(x => x.Type == SocialLink.LinkType.XING).URL;
 
            if (xingURL != SocialLinksService.NO_SOCIAL_LINK)
            {
                var web = new HtmlWeb();
                var doc = web.Load(xingURL);

                IEnumerable<HtmlNode> node = doc.DocumentNode.SelectNodes("//*[@class=\"title ProfilesvCard-jobTitle\"]");
                if (node != null)
                {
                    return node.First().InnerHtml.Replace("\n", "").Trim();
                }
                else
                {
                    return JOB_NOT_FOUND;
                }
            }
            else {
                return JOB_NOT_FOUND;
            }

        }

        private string GetImageURL(string meetingAttendee, List<SocialLink> socialLinks)
        {
            //LinkedIn image search excluded, as it won't return good results!
            bool hasTwitter = !socialLinks.Single(l =>  l.Type == SocialLink.LinkType.TWITTER).URL.Equals(SocialLinksService.NO_SOCIAL_LINK);
            bool hasXing = !socialLinks.Single(l => l.Type == SocialLink.LinkType.XING).URL.Equals(SocialLinksService.NO_SOCIAL_LINK);

            string name = GetNameFromEMailAddress(meetingAttendee);
            string company = GetCompanyFromEMailAddress(meetingAttendee);

            string imgURL = EMPTY_IMG_URL;

            //search for company image first, then xing, then twitter, then for just some rectangular image
            imgURL = _googleCustomSearchService.DoGoogleImageSearch(name, company, ImageType.COMPANY);
            if (!String.IsNullOrEmpty(imgURL)) {
                Console.WriteLine("Company (1) image found for: " + name);
                return imgURL;
            }

            if (hasXing) imgURL = _googleCustomSearchService.DoGoogleImageSearch(name, company, ImageType.XING);
            if (!String.IsNullOrEmpty(imgURL)) {
                Console.WriteLine("Xing (2) image found for: " + name);
                return imgURL;
            }


            if (hasTwitter) imgURL = _googleCustomSearchService.DoGoogleImageSearch(name, company, ImageType.TWITTER);
            if (!String.IsNullOrEmpty(imgURL))
            {
                Console.WriteLine("Twitter (3) image found for: " + name);
                return imgURL;
            }

            imgURL = _googleCustomSearchService.DoGoogleImageSearch(name, company, ImageType.RECTANGULAR);
            if (!String.IsNullOrEmpty(imgURL))
            {
                Console.WriteLine("Rectangular (4) image found for: " + name);
                return imgURL;
            }

            return imgURL;
        
        }

        private string GetNameFromEMailAddress(string emailAddress) {
            
            // get first part of email address and replace . by space (split first and last name)
            string name = emailAddress.Split("@")[0].Replace(".", " ").Trim();

            return name;
        }

        private string GetCompanyFromEMailAddress(string emailAddress)
        {
            // get second part of email address and get only company name
            return emailAddress.Split("@")[1].Split(".")[0];
        }

        private string GetCompanyWebsiteFromEmailAddress(string emailAddress)
        {
            return "https://www." + emailAddress.Split("@")[1];
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

        private string ToTitleCase(string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title);
        }

        public IGoogleCustomSearchService IGoogleCustomSearchService
        {
            get => default(IGoogleCustomSearchService);
            set
            {
            }
        }

        public IGoogleMapsService IGoogleMapsService
        {
            get => default(IGoogleMapsService);
            set
            {
            }
        }

        public DBModelContext DBModelContext
        {
            get => default(DBModelContext);
            set
            {
            }
        }

        public ISocialLinksService ISocialLinksService
        {
            get => default(ISocialLinksService);
            set
            {
            }
        }

        public IGraphService IGraphService
        {
            get => default(IGraphService);
            set
            {
            }
        }


    }
}
