namespace ZambeziDigital.Authentication.Helpers;
public class PasswordGenerator
{
    private const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string SpecialChars = "!@#$%^&*()_-+=[{]};:<>|./?";

    public static string GenerateStrongPassword(int length)
    {
        if (length < 8)
        {
            throw new ArgumentException("Password length must be at least 8 characters.");
        }

        string allChars = LowerCase + UpperCase + Digits + SpecialChars;
        Random random = new Random();
        char[] password = new char[length];

        // Ensure the password has at least one character of each type
        password[0] = LowerCase[random.Next(LowerCase.Length)];
        password[1] = UpperCase[random.Next(UpperCase.Length)];
        password[2] = Digits[random.Next(Digits.Length)];
        password[3] = SpecialChars[random.Next(SpecialChars.Length)];

        // Fill the rest of the password with random characters from the combined list
        for (int i = 4; i < length; i++)
        {
            password[i] = allChars[random.Next(allChars.Length)];
        }

        // Shuffle the password to randomize character positions
        return new string(password.OrderBy(x => Guid.NewGuid()).ToArray());
    }
}