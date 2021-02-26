using System;
using CrmLibrary.Models;
using CrmLibrary.ViewModels;

namespace CRM
{
    public static class ClientService
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
        
        private static ClientModel CreateClient()
        {
            Console.WriteLine("Where * is Required");
            try
            {
                return new ClientModel()
                {
                    Login = Utils.ConsoleWriteWithResult("Login (phone number) *: "),
                    Password = Utils.ConsoleWriteWithResult("Password *: "),
                    DocumentNumber = Utils.ConsoleWriteWithResult("DocumentNumber *: "),
                    Name = Utils.ConsoleWriteWithResult("Name *: "),
                    Surname = Utils.ConsoleWriteWithResult("Surname : "),
                    FathersName = Utils.ConsoleWriteWithResult("Father's Name : "),
                    DateOfBirth = DateTime.Parse(Utils.ConsoleWriteWithResult("DateOfBirth * yyyy-mm-dd: ")),
                    Gender = Utils.ConsoleWriteWithResult("Gender * (M/F) : "),
                    Citizenship = Utils.ConsoleWriteWithResult(@"
Citizenship *:
1.Tajikistan
2.Other
Type 1 or 2".Trim()),
                    MaritalStatus = Utils.ConsoleWriteWithResult(@"
Marital status *:
1. Single
2. Married
3. Divorced
4. Widowed
Type 1, 2, 3 or 4".Trim()),
                    TaxpayerId = Utils.ConsoleWriteWithResult("Taxpayer ID *: "),
                    Address = Utils.ConsoleWriteWithResult("Address *: "),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creating client model error - {ex.Message}");
            }
            return null;
        }

        public static ClientViewModel ClientDbInsert()
        {
            var client = CreateClient();
            return new ClientViewModel();
            //this method will insert created client into db and return its id
            //TODO
        }

        private static FormModel FillInForm(int clientId)
        {
            Console.WriteLine("Where * is Required");
            try
            {
                return new FormModel()
                {
                    ClientId = clientId,
                    CreditHistory = int.Parse(Utils.ConsoleWriteWithResult("Number of closed credits *: ")),
                    Defaults = int.Parse(Utils.ConsoleWriteWithResult("Number of defaults *: ")),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creating form model error - {ex.Message}");
                return null;
            }
        }

        public static FormViewModel FormDbInsert(int clientId)
        {
            FillInForm(clientId);
            return new FormViewModel();
        }
        
        private static CreditApplicationsModel CreateCreditApp(int clientId)
        {
            Console.WriteLine("Where * is Required");
            try
            {
                return new CreditApplicationsModel()
                {
                    ClientId = clientId,
                    Purpose = Utils.ConsoleWriteWithResult("Credit purpose *: "),
                    Amount = decimal.Parse(Utils.ConsoleWriteWithResult("Credit amount *: ")),
                    Period = int.Parse(Utils.ConsoleWriteWithResult("Credit period *: ")),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creating credit application model error - {ex.Message}");
                return null;
            }
        }

        public static CreditApplicationsViewModel CreditAppDbInsert(int clientId)
        {
            CreateCreditApp(clientId);
            return new CreditApplicationsViewModel();
        }
        
        private static bool CreditEvaluate(int clientId, int applicationId)
        {
            return false;
        }
    }
}