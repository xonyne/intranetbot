using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PersonalIntranetBot.UnitTests
{
    public class BingWebSearchServiceMockResults
    {
        public string BingSearchResult_NameAndCompanyMatchingProfilesFound;
        public string BingSearchResult_NameMatchingProfilesFound;
        public string BingSearchResult_NoProfilesFound;

        public BingWebSearchServiceMockResults()
        {
            BingSearchResult_NameAndCompanyMatchingProfilesFound = Properties.Resources.BingSearchResultNameAndCompanyMatchingProfileFound;
            BingSearchResult_NameMatchingProfilesFound = Properties.Resources.BingSearchResultNameMatchingProfileFound;
            BingSearchResult_NoProfilesFound = Properties.Resources.BingSearchResulNoLinkedInProfilesFound;
        }
    }
}
