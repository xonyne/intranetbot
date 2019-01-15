/* 
*  Author: Kevin Suter
*  Description: This class contains all database tables and models of the application.
*  
*/
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace PersonalIntranetBot.Models
{
    public class DBModelContext : DbContext
    {
        public DBModelContext(DbContextOptions<DBModelContext> options)
            : base(options)
        { }

        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<MeetingComment> MeetingComments { get; set; }

        public MeetingComment MeetingComment
        {
            get => default(MeetingComment);
            set
            {
            }
        }

        public SocialLink SocialLink
        {
            get => default(SocialLink);
            set
            {
            }
        }

        public Attendee Attendee
        {
            get => default(Attendee);
            set
            {
            }
        }
    }

    public class Location {
        public int LocationId { get; set; }
        public string LocationString { get; set; }
        public bool IsAddress { get; set; }
    }

    public class Attendee
    {
        public int AttendeeId { get; set; }
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public bool IsPerson { get; set; }
        public ICollection<SocialLink> SocialLinks { get; set; }
        public string ImageURL { get; set; }
        public string CurrentJobTitle { get; set; }
        public string CurrentJobCompany { get; set; }
        public string EducationLocation { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }

    public class MeetingComment
    {
        public int MeetingCommentId { get; set; }
        public string MeetingId { get; set; }
        public string Comment { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }

    public class SocialLink
    {
        public enum LinkType { LINKEDIN, XING, TWITTER, GENERIC };

        public int SocialLinkId { get; set; }
        public int AttendeeId { get; set; }
        public LinkType Type  { get; set; }
        public string URL { get; set; }
    }
}
