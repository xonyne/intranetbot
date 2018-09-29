using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GraphApiConsoleApplication
{
    class Program
    {
        private const string clientId = "1d6d4c56-bfe3-42a4-b9aa-73d1e4f2fc85";
        private const string aadInstance = "https://login.microsoftonline.com/{0}";
        private const string tenant = "headfirebluewin.onmicrosoft.com";
        private const string resource = "https://graph.microsoft.com/";
        private const string appKey = "LPybOGcQmI0J7KG7Qt93OLyw7H56/knhnZC4M+0Ulkg=";
        static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        private static HttpClient httpClient = new HttpClient();
        private static AuthenticationContext context = null;
        private static ClientCredential credential = null;

        static void Main(string[] args)
        {
            context = new AuthenticationContext(authority);
            credential = new ClientCredential(clientId, appKey);

            Task<string> token = GetToken();
            token.Wait();
            Console.WriteLine(token.Result);

            Task<string> events = GetEvents(token.Result);
            events.Wait();
            Console.WriteLine(events.Result);
            Console.ReadLine();
        }

        private static async Task<string> GetEvents(string token)
        {
            string events = null;
            var uri = "https://graph.microsoft.com/v1.0/me/events?$select=subject,body,bodyPreview,organizer,attendees,start,end,location";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var getResult = await httpClient.GetAsync(uri);

            if (getResult.Content != null) {
                events = await getResult.Content.ReadAsStringAsync();
            }

            return events;
        }

        private static async Task<string> GetToken()
        {
            AuthenticationResult result = null;
            string token = null;
            result = await context.AcquireTokenAsync(resource, credential);
            token = result.AccessToken;
            return token;
        }
    }
}
