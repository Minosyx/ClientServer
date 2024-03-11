namespace Wspólne
{
    public static class CommandTools
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
    }
}
