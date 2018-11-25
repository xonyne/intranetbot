using NUnit.Framework;
using Moq;
using System;
using PersonalIntranetBot.Helpers;
using PersonalIntranetBot.Services;

namespace PersonalIntranetBot.UnitTests
{
    [TestFixture]
    public class LinkedInProfileFinderServiceTests
    {
        [Test]
        // Name and company found in description: Kevin Suter - Consultant / Engineer - Zuara AG | LinkedIn
        public void GetLinkedInProfileURLFromNameAndCompany_NameAndCompanyMatchingProfileFound_ReturnsCorrectProfile()
        {
            // Arrange
            string TEST_STRING = "/in/kevin-suter-191372ba";
            string NAME = "Kevin Suter";
            string COMPANY = "Zuara AG";
            string searchString = String.Join(" ", new[] { NAME, COMPANY, LinkedInService.LINKEDIN_SEARCH_SUFFIX });
            Mock<BingWebSearchService> bingWebSearchServiceMock = new Mock<BingWebSearchService>();
            BingWebSearchServiceMockResults bingMockResults = new BingWebSearchServiceMockResults();
            BingWebSearchService.BingSearchResult result = new BingWebSearchService.BingSearchResult { JsonResult = bingMockResults.BingSearchResult_NameAndCompanyMatchingProfilesFound  };
            bingWebSearchServiceMock.Setup(x => x.DoBingWebSearch(searchString)).Returns(result);

            // Act
            string resultString = LinkedInService.GetLinkedInProfileURLFromNameAndCompany(NAME, COMPANY);

            // Assert
            Assert.That(resultString.Contains(TEST_STRING));
        }

        [Test]
        // Only name found in description: Till Jakob -Senior Software Developer - ahead | LinkedIn
        public void GetLinkedInProfileURLFromNameAndCompany_OnlyNameMatchingProfileFound_ReturnsFirstMatch()
        {
            // Arrange
            string TEST_STRING = "/in/till-jakob";
            string NAME = "Till Jakob";
            string COMPANY = "Isolutions";
            string searchString = String.Join(" ", new[] { NAME, COMPANY, LinkedInService.LINKEDIN_SEARCH_SUFFIX });
            Mock<BingWebSearchService> bingWebSearchServiceMock = new Mock<BingWebSearchService>();
            BingWebSearchServiceMockResults bingMockResults = new BingWebSearchServiceMockResults();
            BingWebSearchService.BingSearchResult result = new BingWebSearchService.BingSearchResult { JsonResult = bingMockResults.BingSearchResult_NameMatchingProfilesFound };
            bingWebSearchServiceMock.Setup(x => x.DoBingWebSearch(searchString)).Returns(result);

            // Act
            string resultString = LinkedInService.GetLinkedInProfileURLFromNameAndCompany(NAME, COMPANY);

            // Assert
            Assert.That(resultString.Contains(TEST_STRING));
        }

        [Test]
        // No LinkedIn profiles found at all
        public void GetLinkedInProfileURLFromNameAndCompany_NoProfilesFound_ReturnsEmptyString()
        {
            // Arrange
            string TEST_STRING = "";
            string NAME = "Johnny Depp";
            string COMPANY = "SBB";
            string searchString = String.Join(" ", new[] { NAME, COMPANY, LinkedInService.LINKEDIN_SEARCH_SUFFIX });
            Mock<BingWebSearchService> bingWebSearchServiceMock = new Mock<BingWebSearchService>();
            BingWebSearchServiceMockResults bingMockResults = new BingWebSearchServiceMockResults();
            BingWebSearchService.BingSearchResult result = new BingWebSearchService.BingSearchResult { JsonResult = bingMockResults.BingSearchResult_NameAndCompanyMatchingProfilesFound };
            bingWebSearchServiceMock.Setup(x => x.DoBingWebSearch(searchString)).Returns(result);

            // Act
            string resultString = LinkedInService.GetLinkedInProfileURLFromNameAndCompany(NAME, COMPANY);

            // Assert
            Assert.That(resultString.Contains(TEST_STRING));
        }


    }
}
