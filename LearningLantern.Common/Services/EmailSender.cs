using System.Net;
using System.Net.Mail;
using LearningLantern.Common.Response;

namespace LearningLantern.Common.Services;

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

    public async Task<Response.Response> Send(string emailTo, Message message)
    {
        try
        {
            await Send(emailTo, message.EmailSubject, message.EmailBody, message.IsBodyHtml);
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail(
                new Error
                {
                    ErrorCode = "SendMassageFailed",
                    Description = ex.Message
                }
            );
        }
    }

    private Task Send(string emailTo, string emailSubject, string emailBody, bool isBodyHtml = true)
    {
        var mailMessage = new MailMessage(_fromEmail, emailTo, emailSubject, emailBody)
        {
            IsBodyHtml = isBodyHtml
        };

        return _client.SendMailAsync(mailMessage);
    }
}