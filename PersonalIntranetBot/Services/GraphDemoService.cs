/*
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
*  See LICENSE in the source repository root for complete license information.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace PersonalIntranetBot.Services
{
    public class GraphDemoService : IGraphService
    {
        // Load user's profile in formatted JSON.
        public async Task<string> GetUserJson(GraphServiceClient graphClient, string email, HttpContext httpContext)
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
        public async Task<string> GetPictureBase64(GraphServiceClient graphClient, string email, HttpContext httpContext)
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
        public async Task SendEmail(GraphServiceClient graphClient, IHostingEnvironment hostingEnvironment, string recipients, HttpContext httpContext, string comment, string meetingTitle, string name)
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
                    Content = System.IO.File.ReadAllText(hostingEnvironment.WebRootPath + "/email_template.html").Replace("@comment", comment).Replace("@name", name).Replace("@url", hostingEnvironment.WebRootPath),
                    ContentType = BodyType.Html,
                },
                Subject = "New comment added for " + meetingTitle,
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

        List<Event> IGraphService.GetCalendarEvents(GraphServiceClient graphClient)
        {
            List<Event> graphDemoEvents = new List<Event> {
          new Event {
           Id = "4d4gDDerwesf",
            Subject = "Certified Scrum Master (CSM)®",
            Body = new ItemBody {
             Content = "<div>\n<h4>Inhalt</h4>\nDieser Kurs wird in Englisch durchgeführt werden. The course will be held in English.<br /><br />Scrum is a popular Agile software development method. In this two-day certification course, prepared in accordance with the <a href=\"https://www.scrumalliance.org/\" target=\"_blank\">Scrum Alliances</a> requirements, you will learn the Scrum basics through practical exercises. This course is organized by bbv in cooperation with Crisp AB from Stockholm, Sweden. Crisp AB employs experienced and committed Scrum trainers, among them Henrik Kniberg, author of the standard reference Scrum and XP from the Trenches. The course fee covers the certification fee as well as a two-year membership in the Scrum Alliance. <br /><br />Contents:<br /><ul><li>What are the foundations and principles of Scrum?</li><li>Scrum Framework and meetings: what is time-boxing? What are the roles, rules and artefacts?</li><li>What's the impact of Scrum on my project and organisation? How can I best introduce Scrum considering the required change?</li><li>How can the total cost of ownership (TCO) be measured and optimised?</li><li>What's the best Scrum team composition? What effects of team dynamics need to be considered? What is the influence on team productivity?</li><li>Scrum project planning, predictability, risk management and reporting.</li><li>How can Scrum be applied to large or distributed teams? How can Scrum best be scaled?</li></ul><br /><h4>Ziel</h4>\nAfter attending this course, participants are enabled to apply Scrum in their own projects and are familiar with ideas of how to continuously improve their development processes.<br /><br /><h4>Zielgruppe</h4>\n<ul><li>Test engineers</li><li>Test managers</li><li>Software architects</li><li>Software engineers</li><li>IT project leaders and managers</li><li>QA engineers</li></ul><br /><h4>Voraussetzungen</h4>\nWe recommend that you tackle Scrum prior to the course. The Scrum Alliance has compiled a helpful reading list.</div>"
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
             Content = "<h4>Ablauf</h4>\n<p>Die Verteidigung der Bachelor-Arbeit wird vom Betreuer/von der Betreuerin in Absprache mit dem Experten/der Expertin und den Studierenden organisiert. Der empfohlene Umfang der Verteidigung ist wie folgt:</p>\n<ol>\n<li>\n<p>Vortrag inkl. Demonstration der Ergebnisse <strong>20-40 Minuten&nbsp;<img src=\"https://html-online.com/editor/tinymce4_6_5/plugins/emoticons/img/smiley-innocent.gif\" alt=\"innocent\" /></strong></p>\n</li>\n<li>\n<p>Umfang der Befragung der Studierenden: Anzahl Studierende in der Projektgruppe mal <strong>15 Minuten <img src=\"https://html-online.com/editor/tinymce4_6_5/plugins/emoticons/img/smiley-tongue-out.gif\" alt=\"tongue-out\" /></strong>. ...</p>\n</li>\n</ol>",
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
             }
            },
            IsAllDay = false,
            Recurrence = null
          }
         };
         return graphDemoEvents;
        }
    }
}



