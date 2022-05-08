using System.Net;
using System.Net.Mail;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.Services;

public class EmailSender : IEmailSender
{
    private readonly SmtpClient _client;
    private readonly string _fromEmail;

    public EmailSender(string email, string password, string smtpServerAddress, int mailPort)
    {
        _fromEmail = email;
        _client = new SmtpClient
        {
            Host = smtpServerAddress,
            Port = mailPort,
            EnableSsl = true,
            Credentials = new NetworkCredential(email, password)
        };
    }

    public Task Send(string emailTo, Message message) =>
        Send(emailTo, message.EmailSubject, message.EmailBody, message.IsBodyHtml);

    public Task Send(string emailTo, string emailSubject, string emailBody, bool isBodyHtml = true)
    {
        var mailMessage = new MailMessage(_fromEmail, emailTo, emailSubject, emailBody)
        {
            IsBodyHtml = isBodyHtml
        };

        return _client.SendMailAsync(mailMessage);
    }
}