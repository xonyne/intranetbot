/* 
*  Author: Kevin Suter
*  Description: This class is for testing purposes only. It can be used to mock the GraphService. It will always return a fixed 
*  set of calendar events. Furthermore, notifications are sent to a predefined email address instead of meeting attendees.
*  
*/
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using Newtonsoft.Json;
using PersonalIntranetBot.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Services
{
    public class GraphDemoService : IGraphService
    {
        // Load user's profile in formatted JSON.
        public async Task<string> GetGraphUserJson(GraphServiceClient graphClient, string email, HttpContext httpContext)
        {
            if (email == null) return JsonConvert.SerializeObject(new { Message = "Email address cannot be null." }, Formatting.Indented);

            try
            {
                // Load user profile.
                var user = await graphClient.Users[email].Request().GetAsync();
                return JsonConvert.SerializeObject(user, Formatting.Indented);
            }
            catch (ServiceException e)
            {
                switch (e.Error.Code)
                {
                    case "Request_ResourceNotFound":
                    case "ResourceNotFound":
                    case "ErrorItemNotFound":
                    case "itemNotFound":
                        return JsonConvert.SerializeObject(new { Message = $"User '{email}' was not found." }, Formatting.Indented);
                    case "ErrorInvalidUser":
                        return JsonConvert.SerializeObject(new { Message = $"The requested user '{email}' is invalid." }, Formatting.Indented);
                    case "AuthenticationFailure":
                        return JsonConvert.SerializeObject(new { e.Error.Message }, Formatting.Indented);
                    case "TokenNotFound":
                        await httpContext.ChallengeAsync();
                        return JsonConvert.SerializeObject(new { e.Error.Message }, Formatting.Indented);
                    default:
                        return JsonConvert.SerializeObject(new { Message = "An unknown error has occurred." }, Formatting.Indented);
                }
            }
        }

        // Load user's profile picture in base64 string.
        public async Task<string> GetGraphPictureBase64(GraphServiceClient graphClient, string email, HttpContext httpContext)
        {
            try
            {
                // Load user's profile picture.
                var pictureStream = await GetPictureStream(graphClient, email, httpContext);

                // Copy stream to MemoryStream object so that it can be converted to byte array.
                var pictureMemoryStream = new MemoryStream();
                await pictureStream.CopyToAsync(pictureMemoryStream);

                // Convert stream to byte array.
                var pictureByteArray = pictureMemoryStream.ToArray();

                // Convert byte array to base64 string.
                var pictureBase64 = Convert.ToBase64String(pictureByteArray);

                return "data:image/jpeg;base64," + pictureBase64;
            }
            catch (Exception e)
            {
                switch (e.Message)
                {
                    case "ResourceNotFound":
                        // If picture not found, return the default image.
                        return "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4NCjwhRE9DVFlQRSBzdmcgIFBVQkxJQyAnLS8vVzNDLy9EVEQgU1ZHIDEuMS8vRU4nICAnaHR0cDovL3d3dy53My5vcmcvR3JhcGhpY3MvU1ZHLzEuMS9EVEQvc3ZnMTEuZHRkJz4NCjxzdmcgd2lkdGg9IjQwMXB4IiBoZWlnaHQ9IjQwMXB4IiBlbmFibGUtYmFja2dyb3VuZD0ibmV3IDMxMi44MDkgMCA0MDEgNDAxIiB2ZXJzaW9uPSIxLjEiIHZpZXdCb3g9IjMxMi44MDkgMCA0MDEgNDAxIiB4bWw6c3BhY2U9InByZXNlcnZlIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPg0KPGcgdHJhbnNmb3JtPSJtYXRyaXgoMS4yMjMgMCAwIDEuMjIzIC00NjcuNSAtODQzLjQ0KSI+DQoJPHJlY3QgeD0iNjAxLjQ1IiB5PSI2NTMuMDciIHdpZHRoPSI0MDEiIGhlaWdodD0iNDAxIiBmaWxsPSIjRTRFNkU3Ii8+DQoJPHBhdGggZD0ibTgwMi4zOCA5MDguMDhjLTg0LjUxNSAwLTE1My41MiA0OC4xODUtMTU3LjM4IDEwOC42MmgzMTQuNzljLTMuODctNjAuNDQtNzIuOS0xMDguNjItMTU3LjQxLTEwOC42MnoiIGZpbGw9IiNBRUI0QjciLz4NCgk8cGF0aCBkPSJtODgxLjM3IDgxOC44NmMwIDQ2Ljc0Ni0zNS4xMDYgODQuNjQxLTc4LjQxIDg0LjY0MXMtNzguNDEtMzcuODk1LTc4LjQxLTg0LjY0MSAzNS4xMDYtODQuNjQxIDc4LjQxLTg0LjY0MWM0My4zMSAwIDc4LjQxIDM3LjkgNzguNDEgODQuNjR6IiBmaWxsPSIjQUVCNEI3Ii8+DQo8L2c+DQo8L3N2Zz4NCg==";
                    case "EmailIsNull":
                        return JsonConvert.SerializeObject(new { Message = "Email address cannot be null." }, Formatting.Indented);
                    default:
                        return null;
                }
            }
        }

        // Send an email message from the current user.
        public async Task SendGraphEmail(GraphServiceClient graphClient, IHostingEnvironment hostingEnvironment, string recipients, HttpContext httpContext, string comment, string subject, string author)
        {
            if (recipients == null) return;

            // Prepare the recipient list.
            var splitRecipientsString = recipients.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var recipientList = splitRecipientsString.Select(recipient => new Recipient
            {
                EmailAddress = new EmailAddress
                {
                    Address = recipient.Trim()
                }
            }).ToList();

            // Build the email message.
            var email = new Message
            {
                Body = new ItemBody
                {
                    Content = System.IO.File.ReadAllText(hostingEnvironment.WebRootPath + "/email_template.html").Replace("@comment", comment).Replace("@author", author).Replace("@url", hostingEnvironment.WebRootPath),
                    ContentType = BodyType.Html,
                },
                Subject = subject,
                ToRecipients = recipientList,
            };

            await graphClient.Me.SendMail(email, true).Request().PostAsync();
        }


        private async Task<Stream> GetPictureStream(GraphServiceClient graphClient, string email, HttpContext httpContext)
        {
            if (email == null) throw new Exception("EmailIsNull");

            Stream pictureStream = null;

            try
            {
                try
                {
                    // Load user's profile picture.
                    pictureStream = await graphClient.Users[email].Photo.Content.Request().GetAsync();
                }
                catch (ServiceException e)
                {
                    if (e.Error.Code == "GetUserPhoto") // User is using MSA, we need to use beta endpoint
                    {
                        // Set Microsoft Graph endpoint to beta, to be able to get profile picture for MSAs
                        graphClient.BaseUrl = "https://graph.microsoft.com/beta";

                        // Get profile picture from Microsoft Graph
                        pictureStream = await graphClient.Users[email].Photo.Content.Request().GetAsync();

                        // Reset Microsoft Graph endpoint to v1.0
                        graphClient.BaseUrl = "https://graph.microsoft.com/v1.0";
                    }
                }
            }
            catch (ServiceException e)
            {
                switch (e.Error.Code)
                {
                    case "Request_ResourceNotFound":
                    case "ResourceNotFound":
                    case "ErrorItemNotFound":
                    case "itemNotFound":
                    case "ErrorInvalidUser":
                        // If picture not found, return the default image.
                        throw new Exception("ResourceNotFound");
                    case "TokenNotFound":
                        await httpContext.ChallengeAsync();
                        return null;
                    default:
                        return null;
                }
            }

            return pictureStream;
        }
        private async Task<Stream> GetMyPictureStream(GraphServiceClient graphClient, HttpContext httpContext)
        {
            Stream pictureStream = null;

            try
            {
                try
                {
                    // Load user's profile picture.
                    pictureStream = await graphClient.Me.Photo.Content.Request().GetAsync();
                }
                catch (ServiceException e)
                {
                    if (e.Error.Code == "GetUserPhoto") // User is using MSA, we need to use beta endpoint
                    {
                        // Set Microsoft Graph endpoint to beta, to be able to get profile picture for MSAs
                        graphClient.BaseUrl = "https://graph.microsoft.com/beta";

                        // Get profile picture from Microsoft Graph
                        pictureStream = await graphClient.Me.Photo.Content.Request().GetAsync();

                        // Reset Microsoft Graph endpoint to v1.0
                        graphClient.BaseUrl = "https://graph.microsoft.com/v1.0";
                    }
                }
            }
            catch (ServiceException e)
            {
                switch (e.Error.Code)
                {
                    case "Request_ResourceNotFound":
                    case "ResourceNotFound":
                    case "ErrorItemNotFound":
                    case "itemNotFound":
                    case "ErrorInvalidUser":
                        // If picture not found, return the default image.
                        throw new Exception("ResourceNotFound");
                    case "TokenNotFound":
                        await httpContext.ChallengeAsync();
                        return null;
                    default:
                        return null;
                }
            }

            return pictureStream;
        }

        public List<Event> GetGraphCalendarEvents(GraphServiceClient graphClient)
        {
            List<Event> graphDemoEvents = new List<Event> {
          new Event {
           Id = "4d4gDDerwesf",
            Subject = "Training - Certified Scrum Master (CSM)®",
            Body = new ItemBody {
            Content="<div>Scrum is a popular Agile software development method. In this two-day certification course, prepared in accordance with the <a href=\"\\&quot;https://www.scrumalliance.org/\\&quot;\" target=\"\\&quot;_blank\\&quot;\">Scrum Alliances</a> requirements, you will learn the Scrum basics through practical exercises. <br /><br />Contents:<br />\n<ul>\n<li>What are the foundations and principles of Scrum?</li>\n<li>Scrum Framework and meetings: what is time-boxing? What are the roles, rules and artefacts?</li>\n<li>What's the impact of Scrum on my project and organisation? How can I best introduce Scrum considering the required change?</li>\n<li>How can the total cost of ownership (TCO) be measured and optimised?</li>\n<li>What's the best Scrum team composition? What effects of team dynamics need to be considered? What is the influence on team productivity?</li>\n<li>Scrum project planning, predictability, risk management and reporting.</li>\n<li>How can Scrum be applied to large or distributed teams? How can Scrum best be scaled?</li>\n</ul>\n</div>"
            },
            Location = new Microsoft.Graph.Location {
             Address = new PhysicalAddress() {
              Street = "Blumenrain 10",
              PostalCode = "6003",
              City = "Luzern"
             },
            },
            Start = new DateTimeTimeZone() {
             DateTime = new DateTime(2019, 5, 14, 9, 00, 00).ToString()
            },
            End = new DateTimeTimeZone() {
             DateTime = new DateTime(2019, 5, 15, 17, 00, 00).ToString()
            },
            Attendees = new List < Attendee > {
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "kevin.suter@zuara.ch"
              }
             }
            },
            IsAllDay = false,
            Recurrence = null
          },

          new Event {
           Id = "df4FF344g34345ggggder",
            Subject = "Personal Intranet Bot // Verteidigung Bachelor Thesis Kevin Suter",
            Body = new ItemBody {
             Content = "<h4>Ablauf</h4>\n<p>Die Verteidigung der Bachelor-Arbeit wird vom Betreuer/von der Betreuerin in Absprache mit dem Experten/der Expertin und den Studierenden organisiert. Der empfohlene Umfang der Verteidigung ist wie folgt:</p>\n<ol>\n<li>\n<p>Vortrag inkl. Demonstration der Ergebnisse <strong>20-40 Minuten&nbsp;</strong></p>\n</li>\n<li>\n<p>Umfang der Befragung der Studierenden: Anzahl Studierende in der Projektgruppe mal <strong>15 Minuten</strong>. ...</p>\n</li>\n</ol>",
            },
            Location = new Microsoft.Graph.Location {
             Address = new PhysicalAddress() {
              Street = "Schanzenstrasse 4c",
              PostalCode = "3008",
              City = "Bern"
             },
            },
            Start = new DateTimeTimeZone() {
             DateTime = new DateTime(2019, 2, 6, 18, 00, 00).ToString()
            },
            End = new DateTimeTimeZone() {
             DateTime = new DateTime(2019, 2, 6, 20, 00, 00).ToString()
            },
            Attendees = new List < Attendee > {
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "rolf.gasenzer@bfh.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "pascal.grossniklaus@isolutions.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "thomas.jaeggi@gibb.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "kevin.suter@zuara.ch"
              }
             }
            },
            IsAllDay = false,
            Recurrence = null
          },

          new Event {
           Id = "345DSDFSDddsSERSAEa",
            Subject = "Diplomfeier 2019",
            Body = new ItemBody {
             Content = "<div title=\"Page 2\">\n<div>\n<div>\n<div class=\"eventwrap_singleview\">\n<table class=\"tableEventDetail\">\n<tbody>\n<tr>\n<td class=\"eventDetailFirstColumn\">Datum:</td>\n<td class=\"eventDetailSecondColumn\">08.03.19</td>\n</tr>\n<tr>\n<td class=\"eventDetailFirstColumn\">Zeit:</td>\n<td class=\"eventDetailSecondColumn\">17:00 bis 18:00</td>\n</tr>\n<tr>\n<td class=\"eventDetailFirstColumn\">Ort:</td>\n<td class=\"eventDetailSecondColumn\">Bern</td>\n</tr>\n</tbody>\n</table>\n<table class=\"tableEventDetail\">\n<tbody>\n<tr>\n<td class=\"eventDetailFirstColumn\">Lokalit&auml;t:</td>\n<td class=\"eventDetailSecondColumn\">Stade de Suisse, Champions Lounge, 3. Stock - T&uuml;r&ouml;ffnung 16:30&nbsp;</td>\n</tr>\n</tbody>\n</table>\n</div>\n<div class=\"news-single-item-subheader\">\n<h3>Die Entgegennahme des Diploms ist einer der sch&ouml;nsten Momente f&uuml;r unsere Studierenden.</h3>\n</div>\n<div id=\"c57049\" class=\"csc-default\">\n<ul class=\"linkList\">\n<li><a class=\"external-link-new-window\" title=\"&Ouml;ffnet externen Link in neuem Fenster\" href=\"https://www.google.ch/maps/place/Stade+de+Suisse/@46.9631123,7.4626858,17z/data=!3m1!4b1!4m5!3m4!1s0x478e39fc8d775553:0x3d8c1d6e146b847f!8m2!3d46.9631123!4d7.4648745?hl=de\" target=\"_blank\" rel=\"noopener\">Anfahrtsplan</a></li>\n</ul>\n</div>\n<div id=\"c57050\" class=\"csc-default csc-space-before-10\">\n<table class=\"contenttable-0\" style=\"width: 504px;\">\n<tbody>\n<tr class=\"tr-odd\">\n<td class=\"td-odd\"><strong>Begr&uuml;ssung</strong></td>\n<td class=\"td-even\">Dr. Lukas Rohr<br />Direktor Departement Technik und Informatik</td>\n</tr>\n<tr class=\"tr-even\">\n<td class=\"td-odd\"><strong>Diplom&uuml;bergabe</strong></td>\n<td class=\"td-even\">\n<h3>Bachelor of Science in...</h3>\n<ul>\n<li>Automobiltechnik | Prof. Bernhard Gerster</li>\n<li>Elektrotechnik | Prof. Max Felser</li>\n<li>Informatik | Prof. Dr. Eric Dubuis</li>\n<li>Maschinentechnik | Prof. Roland Hungerb&uuml;hler</li>\n<li>Medizininformatik | Prof. Dr. J&uuml;rgen Holm</li>\n</ul>\n<h3>Master of Science in...</h3>\nEngineering | Prof. Dr. Michael R&ouml;thlin</td>\n</tr>\n<tr class=\"tr-odd\">\n<td class=\"td-odd\">&nbsp;</td>\n<td class=\"td-even\">Geschlossene Gesellschaft</td>\n</tr>\n</tbody>\n</table>\n</div>\n</div>\n</div>\n</div>"
            },
            Location = new Microsoft.Graph.Location {
             Address = new PhysicalAddress() {
              Street = "Stade de Suisse, Bern",
             },
            },
            Start = new DateTimeTimeZone() {
             DateTime = new DateTime(2019, 3, 8, 17, 00, 00).ToString()
            },
            End = new DateTimeTimeZone() {
             DateTime = new DateTime(2019, 3, 8, 18, 00, 00).ToString()
            },
            Attendees = new List < Attendee > {
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "sabine.zumstein@dvbern.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "michel.utz@mobiliar.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "kevin.suter@zuara.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "alexander.nussbaum@mimacom.com"
              }
             }
            },
            IsAllDay = false,
            Recurrence = null
          },
         new Event
         {
             Id = "345DSDFSDddsSERSAEa43D",
             Subject = "ahead Usability Testing",
             Body = new ItemBody
             {
                 Content = "<article class=\"amp-wp-article\">\n<div class=\"amp-wp-article-content\">\n<div class=\"amp-wp-content the_content\">\n<h2>Collaboration Workshops for Leaders and Teams</h2>\n<p>For innovation to flourish, organizations must create an environment that fosters creativity; bringing together multi-talented groups of people who work in close collaboration together&mdash; exchanging knowledge, ideas and shaping the direction of the future. Easier said than done in siloed environments.</p>\n<p>The ability to collaborate across disciplines, business units&nbsp;and customers is crucial to your innovation success &mdash;especially when you are engaged in lean, agile, and/or design thinking processes.&nbsp;Good collaboration fosters original thinking, creativity, and innovation.</p>\n<p><strong>&ldquo;Bad collaboration is worse than no collaboration.&rdquo;</strong> &mdash;Morten T Hansen</p>\n<h3>Explore the principles and practices of effective collaboration in this experiential workshop.</h3>\n<p>Workshops are customized for your organization to help leaders and teams in marketing, sales, HR, engineering, IT, finance, R&amp;D and related functions.</p>\n<h4>Topics include:</h4>\n<ul>\n<li>Assess your collaboration IQ: A self-evaluation tool.</li>\n<li>Collaboration principles and practices</li>\n<li>Establishing rules of engagement for collaboration</li>\n<li>Distinctions between collaboration, cooperation, and consensus</li>\n<li>Identify and overcome the four barriers to collaboration</li>\n<li>Build cross-functional cohesion without losing individualism</li>\n<li>Collaborative leadership practices</li>\n<li>Foster a collaborative culture that values talent, diversity, relationships and connections.</li>\n<li>T shape thinking: Drawing upon the depth of your expertise as well as having the breadth of knowledge to contribute to others</li>\n<li>How to critique an idea and give constructive feedback</li>\n<li>Decision-making</li>\n</ul>\n<p>Workshops are based in part on the research of Morten T Hansen, Warren Bennis, and Google re:Work, as well as <em><a href=\"https://www.creativityatwork.com/orchestrating-collaboration-at-work-toc/\">Orchestrating Collaboration at Work: Using music, improv, storytelling and other arts to improve teamwork</a> </em>(a book on collaboration that was conceived and written collaboratively).</p>\n<h3>Action-based Learning Activities</h3>\n<p>Hands-on activities involve arts-based approaches to create safety, build trust, find shared values, shift perceptions, and uncover the &lsquo;group gold&rsquo; that leads to breakthroughs in problem-solving.&nbsp;The arts are especially useful in helping people from different cultures find common ground and build cohesion.</p>\n<ul>\n<li>Theatre Improv games, such as Yes And vs Yes But to build on ideas rather than tear them apart</li>\n<li>Collaborative poetry writing to solve problems</li>\n<li>Visual -dialogues to communicate through imagery</li>\n<li>Painting a shared vision</li>\n<li>Question-storming and other&nbsp;idea generation&nbsp;techniques</li>\n</ul>\n<p><em>Bonus: No fake team-building exercises.</em></p>\n<p><strong>Learning Outcomes:</strong></p>\n<ul>\n<li>Develop the collaborative leadership and management skills required to create trusting, collaborative environments that inspire and engage diverse groups of people to co-create.</li>\n<li>Build relationships to form networks and self-organizing collegial communities of practice to enhance serendipity and innovation.</li>\n<li>Know when not to collaborate, and avoid wasting time and energy when collaboration is unnecessary.</li>\n<li>Leave the workshop with proven methods for cultivating collaboration in groups and teams.</li>\n</ul>\n<p><strong>Time frame: half-day to two days</strong></p>\n<p>See also <a href=\"https://www.creativityatwork.com/2011/06/03/adventures-in-learning-creativity/\">Adventures in Learning about Creativity</a></p>\n<p class=\"amp-wp-inline-0786f4623984c46e15421e6979be740d\"><strong>How can we help your organization collaborate and innovate?</strong></p>\n<p class=\"amp-wp-inline-0786f4623984c46e15421e6979be740d\">&nbsp;</p>\n<div class=\"fusion-fullwidth fullwidth-box hundred-percent-fullwidth non-hundred-percent-height-scrolling\">\n<div class=\"fusion-builder-row fusion-row \">\n<div class=\"fusion-layout-column fusion_builder_column fusion_builder_column_1_1 fusion-one-full fusion-column-first fusion-column-last fusion-column-no-min-height 1_1 amp-wp-inline-53a0243f03979e11ce67197f1514e600\">\n<div class=\"fusion-column-wrapper\" data-bg-url=\"\">\n<div class=\"fusion-button-wrapper fusion-alignleft\"><a class=\"fusion-button button-flat fusion-button-square button-large button-default button-1\" title=\"H4\" href=\"https://www.creativityatwork.com/contact-us-2/\" target=\"_blank\" rel=\"noopener\"><span class=\"fusion-button-text\">Contact us today </span></a></div>\n<div class=\"fusion-clearfix\">&nbsp;</div>\n</div>\n</div>\n</div>\n</div>\n<div class=\"grammarly-disable-indicator\">&nbsp;</div>\n</div>\n</div>\n</article>\n<div class=\"amp-wp-content widget-wrapper\">\n<div class=\"amp_widget_above_the_footer\">\n<div class=\"textwidget custom-html-widget\">\n<div class=\"orchestrating\"><a href=\"https://www.creativityatwork.com/orchestrating-collaboration-at-work-toc/\"><img class=\"i-amphtml-fill-content i-amphtml-replaced-content\" src=\"https://i2.wp.com/cdn.creativityatwork.com/wp-content/uploads/OCAW-book-cover-270w.jpg?resize=270%2C352&amp;ssl=1\" sizes=\"(min-width: 270px) 270px, 100vw\" alt=\"OCAW-book-cover\" /></a></div>\n<div class=\"orchestrating\"><strong><a href=\"https://www.creativityatwork.com/orchestrating-collaboration-at-work-toc/\">An arts-based recipe book of activities for trainers:</a><a href=\"https://www.creativityatwork.com/orchestrating-collaboration-at-work-toc/\"> Orchestrating Collaboration at Work</a>:</strong><a href=\"https://www.creativityatwork.com/orchestrating-collaboration-at-work-toc/\"> Using music, improv, storytelling and other arts to improve teamwork</a> <a href=\"https://www.creativityatwork.com/orchestrating-collaboration-at-work-toc/\">By Arthur B VanGundy and Linda Naiman</a></div>\n</div>\n</div>\n</div>"
             },
             Location = new Microsoft.Graph.Location {
             Address = new PhysicalAddress() {
              Street = "Schanzenstrasse 4c",
              PostalCode = "3008",
              City = "Bern"
             },
            },
             Start = new DateTimeTimeZone()
             {
                 DateTime = new DateTime(2019, 4, 2, 14, 00, 00).ToString()
             },
             End = new DateTimeTimeZone()
             {
                 DateTime = new DateTime(2019, 4, 2, 18, 00, 00).ToString()
             },
             Attendees = new List<Attendee> {
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "ferdinand.vogler@isolutions.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "frank.quednau@isolutions.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "pascal.grossniklaus@isolutions.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "girolamo.marroccoli@isolutions.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "till.jakob@isolutions.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "peer.juettner@isolutions.ch"
              }
             }
            },
             IsAllDay = false,
             Recurrence = null
         },
         new Event
         {
             Id = "345DSDFSDddsSERSAEa43D43d",
             Subject = "Strategiemeeting 2019",
             Body = new ItemBody
             {
                 Content="<p><strong>Ziele</strong></p>\n<p>Strategische Ausrichtung des Unternehmens f&uuml;r 2019 gemeinsam entwickeln.</p>\n<p><strong>Vorbereitung</strong></p>\n<ol>\n<li>Zahlen 2018 fertigstellen.</li>\n<li>Resum&eacute;e 2018 nach Bereich vorbereiten.</li>\n<li>Pers&ouml;nliche Ziele und W&uuml;nsche f&uuml;r 2019.</li>\n</ol>\n<p><strong>Traktanden</strong></p>\n<ul>\n<li>R&uuml;ckblick 2018\n<ul>\n<li>Welche Ziele wurden erreicht?</li>\n<li>R&uuml;ckblick der einzelnen Bereiche.</li>\n<li>Zahlen 2018 - finanzielle Situation des Unternehmens.</li>\n</ul>\n</li>\n<li>Ausblick 2019\n<ul>\n<li>Wo wollen wir hin im 2019?\n<ul>\n<li>Interne Zusammenarbeit?</li>\n<li>Externe Zusammenarbeit mit Kunden?</li>\n<li>Produkte?</li>\n<li>Weiterbildung?</li>\n<li>Finanzielle Ziele?</li>\n</ul>\n</li>\n<li>Milestones und Planung f&uuml;r 2019 zusammen erarbeiten.</li>\n</ul>\n</li>\n</ul>"
             },
             Location = new Microsoft.Graph.Location
             {
                 Address = new PhysicalAddress()
                 {
                     Street = "Kramgasse 82, 3011 Bern",
                 },
             },
             Start = new DateTimeTimeZone()
             {
                 DateTime = new DateTime(2019, 1, 7, 08, 00, 00).ToString()
             },
             End = new DateTimeTimeZone()
             {
                 DateTime = new DateTime(2019, 1, 7, 20, 00, 00).ToString()
             },
             Attendees = new List<Attendee> {
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "roman.maire@zuara.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "sebastian.fiechter@zuara.ch"
              }
             },
             new Attendee() {
              EmailAddress = new EmailAddress() {
               Address = "kevin.suter@zuara.ch"
              }
             }
            },
             IsAllDay = false,
             Recurrence = null
         },
            };
         return graphDemoEvents;
        }
    }
}



