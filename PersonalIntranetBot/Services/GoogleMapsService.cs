using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Services
{
    public static class GoogleMapsService
    {
        public static String GetGoogleMapsURL(String destination)
        {
            String BASE_URL = "https://www.google.com/maps/dir/?api=1&";
            String destinationEncoded = "destination=" + System.Uri.EscapeDataString(destination) + "&";
            String travelmode = "travelmode=transit";
            return BASE_URL + destinationEncoded + travelmode;
        }
    }
}
