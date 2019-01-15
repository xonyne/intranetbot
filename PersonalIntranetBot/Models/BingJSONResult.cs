/* 
*  Author: Kevin Suter
*  Description: Raw JSON result from Bing Web Search.
*  
*/
using System;
using System.Collections.Generic;

namespace PersonalIntranetBot.Models
{
    public class BingJSONResult
    {
        public String JsonResult { get; set; }
        public Dictionary<String, String> RelevantHeaders { get; set; }
    }
}
