namespace Demo.Common
{
    public class StringHelper
    {
        public static string FirstLetterToUpper(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length > 1)
            {
                return char.ToUpper(str[0]) + str.Substring(1).ToLower();
            }

            return str.ToUpper();
        }
    }
}