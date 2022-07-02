namespace LearningLantern.Common;

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