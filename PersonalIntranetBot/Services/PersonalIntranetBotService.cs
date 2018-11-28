using HtmlAgilityPack;
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
using static PersonalIntranetBot.Models.Attendee;
using static PersonalIntranetBot.Services.SocialLinksService;

namespace PersonalIntranetBot.Services
{
    public static class PersonalIntranetBotService
    {
        public static async Task<List<OutlookEventsViewModel>> GetOutlookCalendarEvents(GraphServiceClient graphClient)
        {
            List<OutlookEventsViewModel> items = new List<OutlookEventsViewModel>();
            Task<IUserEventsCollectionPage> graphEvents = GraphService.GetCalendarEvents(graphClient);
            IUserEventsCollectionPage meetings = await graphEvents;

            if (meetings?.Count > 0)
            {
                foreach (Event currentMeeting in meetings)
                {
                    string meetingLocationAsString = GetAddressFromGraphLocation(currentMeeting.Location);
                    if (MeetingIsNotRecurringAndNotAllDay(currentMeeting) && MeetingIsNotInPast(currentMeeting)) {
                        items.Add(new OutlookEventsViewModel
                        {
                            Id = currentMeeting.Id,
                            Subject = currentMeeting.Subject,
                            Description = currentMeeting.Body.Content,
                            AttendeeEmailAddresses = GetAttendeeEmailAddressesAsList(currentMeeting.Attendees),
                            Start = DateTime.Parse(currentMeeting.Start.DateTime),
                            End = DateTime.Parse(currentMeeting.End.DateTime),
                            Location = meetingLocationAsString,
                            GoogleMapsURL = GoogleMapsService.GetGoogleMapsURL(meetingLocationAsString),
                            Attendees = GetMeetingAttendees(currentMeeting.Attendees),
                        });
                    }
                }
            }
            // Order events by start date ascending
            items = items.OrderBy(e => e.Start).ToList();
            return items;
        }

        private static List<Models.Attendee> GetMeetingAttendees(IEnumerable<Microsoft.Graph.Attendee> graphAttendees) {
            List<Models.Attendee> results = new List<Models.Attendee>();

            foreach (Microsoft.Graph.Attendee a in graphAttendees) {
                string emailAddress = a.EmailAddress.Address.ToString();
                if (emailAddress != null)
                {
                    results.Add(new Models.Attendee
                    {
                        AttendeeId = new Random().Next(1, 10000),
                        EmailAddress = emailAddress,
                        Name = GetNameFromEMailAddress(emailAddress).ToTitleCase(),
                        IsAsPerson = true,
                        SocialLinks = GetSocialLinksForEmailAddress(emailAddress),
                        ImageURL = "",
                        CurrentJobTitle = "",
                        CurrentJobCompany=GetCompanyFromEMailAddress(emailAddress).ToTitleCase(),
                        EducationLocation="",
                    });
                    /*
                    string linkedInProfileURL = LinkedInService.GetLinkedInProfileURLFromNameAndCompany(GetNameFromEMailAddress(emailAddress), GetCompanyFromEMailAddress(emailAddress));
                    if (!String.IsNullOrEmpty(linkedInProfileURL))
                    {
                        results[results.Count - 1].ImageURL = LinkedInService.GetLinkedInProfileInformationChromHeadless(linkedInProfileURL, LinkedInPublicProfileInformation.PROFILEIMAGEURL);
                        results[results.Count - 1].CurrentJobCompany = LinkedInService.GetLinkedInProfileInformationChromHeadless(linkedInProfileURL, LinkedInPublicProfileInformation.CURRENTJOBCOMPANY);
                        results[results.Count - 1].CurrentJobTitle = LinkedInService.GetLinkedInProfileInformationChromHeadless(linkedInProfileURL, LinkedInPublicProfileInformation.CURRENTJOBTITLE);
                        results[results.Count - 1].EducationLocation = LinkedInService.GetLinkedInProfileInformationChromHeadless(linkedInProfileURL, LinkedInPublicProfileInformation.EDUCATIONLOCATION);
                    }
                    */
                }

            }
            return results;
        }

        private static string GetNameFromEMailAddress(string emailAddress) {
            // get first part of email address and replace . by space (split first and last name)
            return emailAddress.Split("@")[0].Replace(".", " ").Trim();
        }

        private static string GetCompanyFromEMailAddress(string emailAddress)
        {
            // get second part of email address and get only company name
            return emailAddress.Split("@")[1].Split(".")[0];
        }


        private static bool MeetingIsNotRecurringAndNotAllDay(Event meeting)
        {
            return (!(bool)meeting.IsAllDay && meeting.Recurrence == null);
        }

        private static bool MeetingIsNotInPast(Event meeting)
        {
            return (DateTime.Parse(meeting.End.DateTime) > System.DateTime.Now);
        }

        private static List<SocialLink> GetSocialLinksForEmailAddress(String emailAddress)
        {
            List<SocialLink> results = new List<SocialLink>
            {
                new SocialLink
                {
                    Type = SocialLink.LinkType.LINKEDIN,
                    URL = SocialLinksService.GetLinkedInAccountURLFromNameAndCompany(GetNameFromEMailAddress(emailAddress), GetCompanyFromEMailAddress(emailAddress))
                },
                new SocialLink
                {
                    Type = SocialLink.LinkType.XING,
                    URL = SocialLinksService.GetXingAccountURLFromNameAndCompany(GetNameFromEMailAddress(emailAddress), GetCompanyFromEMailAddress(emailAddress))
                },
                new SocialLink
                {
                    Type = SocialLink.LinkType.TWITTER,
                    URL = SocialLinksService.GetTwitterAccountURLFromNameAndCompany(GetNameFromEMailAddress(emailAddress), GetCompanyFromEMailAddress(emailAddress))
                }
            };
            return results;

        }

        private static String GetAttendeeEmailAddressesAsString(this IEnumerable<Microsoft.Graph.Attendee> collection, String separator)
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

        private static List<string> GetAttendeeEmailAddressesAsList(this IEnumerable<Microsoft.Graph.Attendee> collection)
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

        private static String GetAddressFromGraphLocation(Location location)
        {
            if (location.Address != null)
            {
                String street = String.IsNullOrEmpty(location.Address.Street) ? "" : location.Address.Street;
                String postalCode = String.IsNullOrEmpty(location.Address.PostalCode) ? "" : ", " + location.Address.PostalCode + " ";
                String city = String.IsNullOrEmpty(location.Address.City) ? "" : location.Address.City;
                return street + postalCode + city;
            } 
            if (location.DisplayName != null) {
                return location.DisplayName;
}
            return "";
        }
        
        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title);
        }

    }
}
