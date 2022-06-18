using LearningLantern.Common.Responses;

namespace LearningLantern.Common.Services;

public interface IEmailSender
{
    public Task<Response> Send(string toEmail, Message message);
}