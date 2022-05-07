namespace LearningLantern.ApiGateway.Configurations;

public static class Pattern
{
    public const string Name = "^((?![0-9!\"#$%&'()*+,-./\\:;<=>?@[" + @"\]" + "^_`{|}~]).){2,30}$";

    public const string Password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[ !\"#$%&'()*+,-./\\:;<=>?@[" + @"\]" +
                                   "^_`{|}~])[a-zA-Z0-9 !\"#$%&'()*+,-./\\:;<=>?@[" + @"\]" + "^_`{|}~]{6,}$";
}