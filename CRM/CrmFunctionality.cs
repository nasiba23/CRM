using System;

namespace CRM
{
    public static class CrmFunctionality
    {
        public static string ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine($"{new string('-', 5)}Main menu{new string('-', 5)}");
            Console.Write("1. Registration\n" +
                          "2. Client login\n" +
                          "3. Admin login\n" +
                          "4. Exit\n" +
                          "Choice:");
            return Console.ReadLine();
        }
    }
}