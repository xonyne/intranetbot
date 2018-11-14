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
                            GoogleMapsURL = GoogleMapsURLService.getGoogleMapsURL(meetingLocationAsString),
                            LinkedIdProfileURLs = GetLinkedInProfileURLsFromEmailAddresses(GetAttendeeEmailAddressesAsString(currentMeeting.Attendees, ", ")),
                        });
                    }
                }
            }
            return items;
        }

        private static bool MeetingIsNotRecurringAndNotAllDay(Event meeting)
        {
            return (!(bool)meeting.IsAllDay && meeting.Recurrence == null);
        }

        private static bool MeetingIsNotInPast(Event meeting)
        {
            return (DateTime.Parse(meeting.End.DateTime) > System.DateTime.Now);
        }

        private static Dictionary<string, string> GetLinkedInProfileURLsFromEmailAddresses(String emailAddresses)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            string[] arrAddresses = emailAddresses.Split(',');
            foreach (string address in arrAddresses)
            {
                if (!String.IsNullOrEmpty(address))
                {
                    // get first part of email address and replace . by space (split first and last name)
                    string name = address.Split("@")[0].Replace(".", " ").Trim();
                    // get second part of email address and get only company name
                    string company = address.Split("@")[1].Split(".")[0];
                    string linkedInProfileURL = LinkedInProfileFinderService.GetLinkedInProfileURLFromNameAndCompany(name, company);
                    //string linkedInProfileImageURL = GetLinkedInProfileImageURL(linkedInProfileURL);
                    // artificial slow down, because Bing does not allow more than 5 requests per second.
                    Thread.Sleep(500);
                    results.Add(name.ToTitleCase(), linkedInProfileURL);
                }
            }
            return results;
        }

        private static String GetAttendeeEmailAddressesAsString(this IEnumerable<Attendee> collection, String separator)
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

        private static List<string> GetAttendeeEmailAddressesAsList(this IEnumerable<Attendee> collection)
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

        private static string GetLinkedInProfileImageURL(string linkedInProfileURL)
        {
            // does not work at the moment due to login
            return "";

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(linkedInProfileURL);
            HtmlNode linkedInProfileImageNode = doc.DocumentNode.SelectNodes("//*[@id=\"ember66\"]")[0];
            string linkedInProfileImageURL = "";
            if (linkedInProfileImageNode != null)
            {
                string wholeAttributeValue = linkedInProfileImageNode.Attributes["style"].Value;
                int startIndex = wholeAttributeValue.IndexOf("url(\"", StringComparison.Ordinal);
                int endIndex = wholeAttributeValue.IndexOf("\")", StringComparison.Ordinal);
                linkedInProfileImageURL = wholeAttributeValue.Substring(startIndex, wholeAttributeValue.Length - endIndex);
            }
            return linkedInProfileImageURL;
        }
    }
}
