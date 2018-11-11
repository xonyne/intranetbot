using Microsoft.Graph;
using PersonalIntranetBot.Helpers;
using PersonalIntranetBot.Models;
using System;
using System.Collections.Generic;
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
            IUserEventsCollectionPage events = await graphEvents;

            if (events?.Count > 0)
            {
                foreach (Event current in events)
                {
                    string participantEmailAddressesAsString = getAttendeeEmailAddressesAsString(current.Attendees, ", ");
                    string meetingLocationAsString = getAddressFromGraphLocation(current.Location);
                    items.Add(new OutlookEventsViewModel
                    {
                        Id = current.Id,
                        Subject = current.Subject,
                        Description = current.Body.Content,
                        AttendeeEmailAddresses = participantEmailAddressesAsString,
                        Start = DateTime.Parse(current.Start.DateTime),
                        End = DateTime.Parse(current.End.DateTime),
                        Location = meetingLocationAsString,
                        GoogleMapsURL = GoogleMapsURLService.getGoogleMapsURL(meetingLocationAsString),
                        LinkedIdProfileURLs = GetLinkedInProfileURLsFromEmailAddresses(participantEmailAddressesAsString),
                    });
                }
            }
            return items;
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
                    // artificial slow down, because Bing does not allow more than 5 requests per second.
                    Thread.Sleep(500);
                    results.Add(name + "(" + company + ")", linkedInProfileURL);
                }
            }
            return results;
        }

        private static String getAttendeeEmailAddressesAsString(this IEnumerable<Attendee> collection, String seperator)
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
                    builder.Append(seperator).Append(enumerator.Current.EmailAddress.Address);
                }

                return builder.ToString();
            }
        }

        private static String getAddressFromGraphLocation(Location location)
        {
            if (location.Address != null)
            {
                String street = String.IsNullOrEmpty(location.Address.Street) ? "" : location.Address.Street;
                String postalCode = String.IsNullOrEmpty(location.Address.PostalCode) ? "" : ", " + location.Address.PostalCode + " ";
                String city = String.IsNullOrEmpty(location.Address.City) ? "" : location.Address.City;
                return street + postalCode + city;
            }
            return "";
        }

        private static List<string> getAttendeeEmailAddresses(String emailAddresses)
        {
            return new List<string>();
        }
    }
}
