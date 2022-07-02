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
    public static int MailPort => Get("Email:MailPort", 587);

    //RabbitMQ
    public static Uri RabbitMQUri => Get<Uri>("RabbitMQ:Uri");

    //Calendar
    public static Uri CalendarGetEventsEndPoint => Get<Uri>("Calendar:GetEventsEndPoint");

    //helpers
    private static string Get(string name) => Get<string>(name);

    private static T Get<T>(string name) =>
        Configuration.GetValue<T>(name) ?? throw new Exception("Configuration for " + name + " not found");

    private static T Get<T>(string name, T defaultValue) => Configuration.GetValue<T>(name) ?? defaultValue;
}