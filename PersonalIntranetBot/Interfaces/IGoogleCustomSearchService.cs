namespace PersonalIntranetBot.Interfaces
{
    /* Identifies the type of a profile image of a meeting attendee */
    public enum ImageType
    {
        COMPANY,
        XING,
        TWITTER,
        RECTANGULAR
    }

    public interface IGoogleCustomSearchService
    {
        string DoGoogleImageSearch(string name, string company, ImageType imageType);
    }
}
