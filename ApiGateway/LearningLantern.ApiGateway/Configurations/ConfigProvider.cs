namespace LearningLantern.ApiGateway.Configurations;

public static class ConfigProvider
{
    public static IConfiguration Configuration = null!;

    public static string ConfirmationEmailEndpoint => Get("ConfirmationEmailEndpoint");
    //Database
    public static string DefaultConnectionString => Get("ConnectionStrings:Default");

    //Email
    public static string MyEmail => Get("Email:Email", "");
    public static string MyPassword => Get("Email:Password", "");
    public static string SmtpServerAddress => Get("Email:SMTPServerAddress", "smtp.gmail.com");
    public static int MailPort => Convert.ToInt32(Get("Email:MailPort", "587"));

    //RabbitMQ
    public static string RabbitMQHost => Get("RabbitMQ:Host");
    public static int RabbitMQPortNumber => Convert.ToInt32(Get("RabbitMQ:Port", "5672"));


    //helpers
    private static string Get(string name) =>
        Configuration[name] ?? throw new Exception("Configuration for " + name + " not found");

    private static string Get(string name, string defaultValue) => Configuration[name] ?? defaultValue;
}