using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.Services;

public interface IEmailSender
{
    public Task Send(string emailTo, Message message);
}