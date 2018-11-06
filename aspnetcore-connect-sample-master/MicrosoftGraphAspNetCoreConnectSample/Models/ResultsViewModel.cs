using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PersonalIntranetBot.Models
{

    // An entity, such as a user, group, or message.
    public class ResultsItem
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        [DisplayFormat(DataFormatString = "{0:ddd, dd.MM.yy HH:mm}")]
        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:ddd, dd.MM.yy HH:mm}")]
        public DateTime End { get; set; }
        public string Location { get; set; }
        public string AttendeeEmailAddresses { get; set; }
        //public List<string> AttendeeEmailAddresses { get; set; }
        public string GoogleMapsURL { get; set; }
        // Key: Name of person, Value: URL to LinkedIn profile (empty string if not found)
        public Dictionary<string, string> LinkedIdProfileURLs { get; set; }


        // The properties of an entity that display in the UI.
        public Dictionary<string, object> Properties;

        public ResultsItem()
        {
            Properties = new Dictionary<string, object>();
        }
    }

    // View model to display a collection of one or more entities returned from the Microsoft Graph. 
    public class ResultsViewModel
    {

        // Set to false if you don't want to display radio buttons with the results.
        public bool Selectable { get; set; }

        // The list of entities to display.
        public IEnumerable<ResultsItem> Items { get; set; }
        public ResultsViewModel(bool selectable = true)
        {

            // Indicates whether the results should display radio buttons.
            // This is how an entity ID is passed to methods that require it.
            Selectable = selectable;

            Items = Enumerable.Empty<ResultsItem>();
        }
    }
}