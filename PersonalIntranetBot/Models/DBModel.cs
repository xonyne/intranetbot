using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Models
{
    public class DBModelContext : DbContext
    {
        public DBModelContext(DbContextOptions<DBModelContext> options)
            : base(options)
        { }

        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
    }

    public class Attendee
    {
        public int AttendeeId { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public bool IsAsPerson { get; set; }
        public ICollection<SocialLink> SocialLinks { get; set; }
    }

    public class SocialLink
    {
        public enum LinkType { LINKEDIN, XING, FACEBOOK, INSTAGRAM, TWITTER, GENERIC };

        public int SocialLinkId { get; set; }
        public int AttendeeId { get; internal set; }
        public LinkType Type  { get; set; }
        public string URL { get; set; }
    }
}
