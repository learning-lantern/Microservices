using LearningLantern.Common;

namespace LearningLantern.ApiGateway.Utility;

public static class MessageTemplates
{
    private const string ConfirmationEmailEndpoint = "https://learning-lantern.web.app/auth/validate-email";

    public static Message ConfirmationEmail(string userId, string token)
    {
        var confirmEmailRoute = $"{ConfirmationEmailEndpoint}?userId={userId}&token={token}";

        var emailBody = "<h1>Welcome To Learning Lantern</h1><br>" +
                        "<p> Thanks for registering at learning lantern please click " +
                        $"<strong><a href=\"{confirmEmailRoute}\" target=\"_blank\">here</a></strong>" +
                        " to confirmation your email</p>";

        return new Message("Confirmation Email", emailBody, true);
    }

    public static Message ChangeEmail(string userId, string newEmail, string token)
    {
        var confirmEmailRoute = $"{ConfirmationEmailEndpoint}?userId={userId}&newEmail={newEmail}&token={token}";

        var emailBody =
            $"<p> please click <strong><a href=\"{confirmEmailRoute}\" target=\"_blank\">here</a></strong>" +
            " to confirmation your new email for your <strong>Learning Lantern</strong> Account</p>";

        return new Message("Confirmation The new Email", emailBody, true);
    }
}