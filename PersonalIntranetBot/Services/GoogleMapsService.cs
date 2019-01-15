/* 
*  Author: Kevin Suter
*  Description: This class is used to generate the Google Maps URL link for travel information.
*  
*/
using PersonalIntranetBot.Interfaces;
using System;

namespace PersonalIntranetBot.Services
{
    public class GoogleMapsService : IGoogleMapsService
    {
        public String GetGoogleMapsURL(String destination)
        {
            String BASE_URL = "https://www.google.com/maps/dir/?api=1&";
            String destinationEncoded = "destination=" + System.Uri.EscapeDataString(destination) + "&";
            String travelmode = "travelmode=transit";
            return BASE_URL + destinationEncoded + travelmode;
        }
    }
}
