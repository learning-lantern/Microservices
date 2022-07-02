namespace LearningLantern.ApiGateway.Utility;

public static class Pattern
{
    public const string Name = "^((?![0-9!\"#$%&'()*+,-./\\:;<=>?@[" + @"\]" + "^_`{|}~]).){2,30}$";

    /// <summary>
    ///     Password property that its pattern must consist of a minimum 6 characters and maximum 30,
    ///     at least one uppercase letter, one lowercase letter, and one number.
    /// </summary>
    public const string Password = "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{6,30}$";

    /*
    /// <summary>
    ///     Password property that its pattern must consist of a minimum 6 characters, at least one uppercase letter, one
    ///     lowercase letter, one special character, and one number.
    /// </summary>
    public const string Password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[ !\"#$%&'()*+,-./\\:;<=>?@[" + @"\]" +
                                   "^_`{|}~])[a-zA-Z0-9 !\"#$%&'()*+,-./\\:;<=>?@[" + @"\]" + "^_`{|}~]{6,}$";
    */
}