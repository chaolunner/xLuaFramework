using System;

namespace UniEasy
{
    public static partial class StringExtensions
    {
        public static string[] Split(string content, params string[] separator)
        {
            return content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
