namespace Wspólne
{
    public static class CommonTools
    {
        public static string GenerateRandomAnswer(int length)
        {
            char[] chars = new char[length];
            int m = 'Z' - 'A' + 1;
            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)('A' + i % m);
            }
            return new string(chars);
        }

        public static string SubstringMax(this string str, int len)
        {
            return str[..Math.Min(str.Length, len)];
        }
    }
}
