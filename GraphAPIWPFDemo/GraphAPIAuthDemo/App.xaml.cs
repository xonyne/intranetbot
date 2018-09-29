using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GraphAPIWPFDemo
{
    public partial class App : Application
    {
        //Below is the clientId of your app registration. 
        //You have to replace the below with the Application Id for your app registration
        private static string ClientId = "eca8140f-07d1-442e-bdd3-3c4869fac105";

        public static PublicClientApplication PublicClientApp = new PublicClientApplication(ClientId);

    }
}
