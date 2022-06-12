namespace LearningLantern.ApiGateway.Utility;

public static class MessageTemplates
{
    public static Message ConfirmationEmail(string? endPointUrl, string userId, string token)
    {
        if (endPointUrl == null) throw new ArgumentNullException(nameof(endPointUrl));

        var confirmEmailRoute = $"{endPointUrl}?userId={userId}&token={token}";

        var emailBody = "<h1>Welcome To Learning Lantern</h1><br>" +
                        "<p> Thanks for registering at learning lantern please click " +
                        $"<strong><a href=\"{confirmEmailRoute}\" target=\"_blank\">here</a></strong>" +
                        " to activate your account</p>";

        return new Message("Confirmation Email", emailBody, true);
    }
}

public class Message
{
    public Message(string emailSubject, string emailBody, bool isBodyHtml)
    {
        EmailSubject = emailSubject;
        EmailBody = emailBody;
        IsBodyHtml = isBodyHtml;
    }

    public string EmailSubject { get; }
    public string EmailBody { get; }

    public bool IsBodyHtml { get; }
}