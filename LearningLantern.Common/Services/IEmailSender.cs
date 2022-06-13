namespace LearningLantern.Common.Services;

public interface IEmailSender
{
    public Task<Response.Response> Send(string emailTo, Message message);
}