/* 
*  Author: Microsoft
*  Description: The application is started from this class.
*  
*/
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace PersonalIntranetBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
        BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .Build();
    }
}
