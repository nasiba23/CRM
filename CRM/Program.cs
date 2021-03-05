using System;
using System.Net;
using System.Threading.Tasks;
using CrmLibrary.Models;

namespace CRM
{
    class Program
    {
        private static bool _isWorking = true;
        static async Task Main(string[] args)
        {
            while (_isWorking)
            {
                switch (Utils.ShowMainMenu())
                {
                    //register and credit processing
                    case "1":
                    {
                        await ClientService.CreditProcess();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Press any key to return main menu");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                        break;
                    //client login
                    case "2":
                    {
                        
                    }
                        break;
                    //admin login
                    case "3":
                    {
                        await AdminService.Authorization();
                    }
                        break;
                    //exit application
                    case "4": _isWorking = false;
                        break;
                }
            }
        }
    }
}