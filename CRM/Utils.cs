using System;

namespace CRM
{
    /// <summary>
    /// methods used in different parts of application
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// displays main menu
        /// </summary>
        /// <returns>user's input</returns>
        public static string ShowMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{new string(' ', 5)}Main menu{new string(' ', 5)}");
            Console.ResetColor();
            Console.Write("1. Registration and credit processing for clients\n" +
                          "2. Login for clients\n" +
                          "3. Login for admins\n" +
                          "4. Exit\n" +
                          "Choice: ");
            return Console.ReadLine();
        }
        /// <summary>
        /// asks user to input text and returns it as result
        /// </summary>
        /// <param name="text">text to be shown in console</param>
        /// <returns>user's input</returns>
        public static string ConsoleWriteWithResult(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }
        
        /// <summary>
        ///prints message to indicate required fields
        /// </summary>
        public static void RequiredWriter()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Where * is Required");
            Console.ResetColor();
        }
        
        /// <summary>
        /// notifies of instance successful creation
        /// </summary>
        /// <param name="id">instance id</param>
        public static void CreationNotify(int id)
        {
            if (id != 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully created");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while creating");
                Console.ResetColor();
            }
        }
    }
}