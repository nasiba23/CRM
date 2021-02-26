using System;

namespace CRM
{
    public static class Utils
    {
        public static string ConsoleWriteWithResult(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }
    }
}