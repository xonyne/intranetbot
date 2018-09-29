using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using AdaptiveCards;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Drawing;

namespace Intranet_Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
            return Task.CompletedTask;
        }
        private readonly IDictionary<string, string> options = new Dictionary<string, string>
        {
 { "1", "1. Show Demo Adaptive Card " },
    { "2", "2. Show Demo for Adaptive Card Design with Column" },  
 {"3" , "3. Input Adaptive card Design" }  
     };  

     public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
{
    var message = await result;
    var welcomeMessage = context.MakeMessage();
    welcomeMessage.Text = "Welcome to bot Adaptive Card Demo";

    await context.PostAsync(welcomeMessage);

    this.DisplayOptionsAsync(context);
}

public void DisplayOptionsAsync(IDialogContext context)
{
    PromptDialog.Choice<string>(
        context,
        this.SelectedOptionAsync,
        this.options.Keys,
        "What Demo / Sample option would you like to see?",
        "Please select Valid option 1 to 6",
        6,
        PromptStyle.PerLine,
        this.options.Values);
}
public async Task SelectedOptionAsync(IDialogContext context, IAwaitable<string> argument)
{
    var message = await argument;

    var replyMessage = context.MakeMessage();

    Attachment attachment = null;

    switch (message)
    {
        case "1":
            attachment = CreateAdapativecard();
            replyMessage.Attachments = new List<Attachment> { attachment };
            break;
        case "2":
                    attachment = CreateAdapativecard();
                    //attachment = CreateAdapativecardWithColumn();
                    replyMessage.Attachments = new List<Attachment> { attachment };
            break;
        case "3":
                    attachment = CreateAdapativecard();
                    //replyMessage.Attachments = new List<Attachment> { CreateAdapativecardWithColumn(), CreateAdaptiveCardwithEntry() };
                    break;

    }


    await context.PostAsync(replyMessage);

    this.DisplayOptionsAsync(context);
}


public Attachment CreateAdapativecard()
{

    AdaptiveCard card = new AdaptiveCard();

    // Specify speech for the card.  
    card.Speak = "Suthahar J is a Technical Lead and C# Corner MVP. He has extensive 10+ years of experience working on different technologies, mostly in Microsoft space. His focus areas are  Xamarin Cross Mobile Development ,UWP, SharePoint, Azure,Windows Mobile , Web , AI and Architecture. He writes about technology at his popular blog http://devenvexe.com";
    // Body content  
    card.Body.Add(new AdaptiveImage()
    {
        Url = new Uri("https://i1.social.s-msft.com/profile/u/avatar.jpg?displayname=j%20suthahar&size=extralarge&version=88034ca2-9db8-46cd-b767-95d17658931a"),
        Size = AdaptiveImageSize.Small,
        Style = AdaptiveImageStyle.Person,
        AltText = "Suthahar Profile"

    });

    // Add text to the card.  
    card.Body.Add(new AdaptiveTextBlock()
    {
        Text = "Technical Lead and C# Corner MVP",
        Size = AdaptiveTextSize.Large,
        Weight = AdaptiveTextWeight.Bolder
    });

    // Add text to the card.  
    card.Body.Add(new AdaptiveTextBlock()
    {
        Text = "jssutahhar@gmail.com"
    });

    // Add text to the card.  
    card.Body.Add(new AdaptiveTextBlock()
    {
        Text = "97XXXXXX12"
    });

    // Create the attachment with adapative card.  
    Attachment attachment = new Attachment()
    {
        ContentType = AdaptiveCard.ContentType,
        Content = card
    };
    return attachment;
}
    }
}