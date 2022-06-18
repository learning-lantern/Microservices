using System.Net;
using System.Net.Mail;
using LearningLantern.Common.Responses;

namespace LearningLantern.Common.Services;

public class EmailSender : IEmailSender
{
    private readonly SmtpClient client;
    private readonly string fromEmail;

    public EmailSender(string email, string password, string smtpServerAddress, int mailPort)
    {
        fromEmail = email;

        client = new SmtpClient
        {
            Host = smtpServerAddress,
            Port = mailPort,
            EnableSsl = true,
            Credentials = new NetworkCredential(email, password)
        };
    }

    public async Task<Response> Send(string toEmail, Message message)
    {
        try
        {
            await Send(toEmail, message.EmailSubject, message.EmailBody, message.IsBodyHtml);
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

    private async Task Send(string emailTo, string emailSubject, string emailBody, bool isBodyHtml = true)
    {
        var mailMessage = new MailMessage(fromEmail, emailTo, emailSubject, emailBody)
        {
            IsBodyHtml = isBodyHtml
        };

        await client.SendMailAsync(mailMessage);
    }
}