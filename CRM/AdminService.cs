using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrmLibrary.Models;

namespace CRM
{
    /// <summary>
    /// methods for admin logic
    /// </summary>
    public static class AdminService
    {
        /// <summary>
        /// authorizes user
        /// </summary>
        /// <returns>Task</returns>
        public static async Task AdminAuthorization()
        {
            Console.Clear();
            string login = Utils.ConsoleWriteWithResult("Login: ");
            string password = Utils.ConsoleWriteWithResult("Password: ");
            try
            {
                var admin = await DatabaseService.AdminAuthorizationCheck(login, password);
                if (admin.Id > 0 && !string.IsNullOrEmpty(admin.Name))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{admin.Name}, you are successfully logged in");
                    Console.ResetColor();
                    await Task.Delay(1500);
                    await ShowAdminMenu(admin.Id);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect login or password, press any key to return to main menu");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authorization error {ex.Message}");
            }
        }
        /// <summary>
        /// displays admin menu
        /// </summary>
        /// <returns></returns>
        private static async Task ShowAdminMenu(int id)
        {
            var isWorking = true;
            while (isWorking)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{new string(' ', 5)}Admin menu{new string(' ', 5)}");
                Console.ResetColor();
                Console.Write("1. Register new admin\n" +
                              "2. Change password\n" +
                              "3. View credit applications\n" +
                              "4. Log out\n" +
                              "Choice: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    //register new admin
                    case "1":
                    {
                        await CreateAdminAsync();
                    }
                        break;
                    //change password
                    case "2":
                    {
                        await ChangePasswordAsync(id);
                    }
                        break;
                    //view credit applications
                    case "3":
                    {
                        await ViewCreditApplications();
                    }
                        break;
                    //log out
                    case "4":
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Logging out...");
                        Console.ResetColor();
                        await Task.Delay(2000);
                        isWorking = false;
                    }
                        break;
                }
            }
        }

        /// <summary>
        /// creates admin
        /// </summary>
        /// <returns>Task</returns>
        private static async Task CreateAdminAsync()
        {
            Utils.RequiredWriter();
            try
            {
                var admin = new AdminModel()
                {
                    Login = Utils.ConsoleWriteWithResult("Login (phone number) *: "),
                    Password = Utils.ConsoleWriteWithResult("Password *: "),
                    DocumentNumber = Utils.ConsoleWriteWithResult("DocumentNumber *: "),
                    Name = Utils.ConsoleWriteWithResult("Name *: "),
                    Surname = Utils.ConsoleWriteWithResult("Surname : "),
                    FathersName = Utils.ConsoleWriteWithResult("Father's Name : "),
                    DateOfBirth = DateTime.Parse(Utils.ConsoleWriteWithResult("DateOfBirth * yyyy-mm-dd: ")),
                    Gender = Utils.ConsoleWriteWithResult("Gender * (M/F) : ").ToUpper(),
                    Citizenship = Utils.ConsoleWriteWithResult("" +
                                                               "Citizenship *:\n" +
                                                               "1. Tajikistan\n" +
                                                               "2. Other\n" +
                                                               "Type 1 or 2 : "
                                                               ),
                    MaritalStatus = Utils.ConsoleWriteWithResult("" +
                                                                 "Marital status *:\n" +
                                                                 "1. Single\n" +
                                                                 "2. Married\n" +
                                                                 "3. Divorced\n" +
                                                                 "4. Widowed\n" +
                                                                 "Type 1, 2, 3, or 4: "
                                                                 ),
                    TaxpayerId = Utils.ConsoleWriteWithResult("Taxpayer ID *: "),
                    Address = Utils.ConsoleWriteWithResult("Address *: "),
                };
                await DatabaseService.InsertIntoAdminsAsync(admin);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Admin successfully created, going back to main menu in 5 second...");
                await Task.Delay(5000);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creating admin model error - {ex.Message}");
            }
        }

        /// <summary>
        /// changes password of logged in admin
        /// </summary>
        /// <param name="id">admin's id</param>
        /// <returns>Task</returns>
        private static async Task ChangePasswordAsync(int id)
        {
            Console.Clear();
            var currentPassword = Utils.ConsoleWriteWithResult("Current password: ");
            var newPassword = Utils.ConsoleWriteWithResult("New password: ");
            try
            {
                var result = await DatabaseService.UpdatePasswordInDbAsync(id, currentPassword, newPassword);
                //check if any row was affected
                if (result == 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect current password, try again");
                    await Task.Delay(2000);
                    Console.ResetColor();
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Password successfully updated, going back to admin menu in 5 seconds...");
                    await Task.Delay(5000);
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Changing password error {ex.Message}");
            }
        }

        /// <summary>
        /// shows all credit applications
        /// </summary>
        /// <returns>Task</returns>
        private static async Task ViewCreditApplications()
        {
            try
            {
                var listApps = new List<CreditApplicationsModel>();
                listApps = await DatabaseService.SelectAllApplicationsFromDb();
                int temp = 0;
                foreach (var item in listApps)
                {
                    var creditApp  = listApps[temp];
                    temp++;
                    Type type = typeof(CreditApplicationsModel);
                    for (int i = 0; i < type.GetProperties().Length; i++)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{type.GetProperties()[i].Name}: ");
                        Console.ResetColor();
                        Console.Write($"{type.GetProperties()[i].GetValue(creditApp)}, ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Going back to admin menu in 5 seconds...");
                await Task.Delay(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Viewing credit applications error {ex.Message}");
                await Task.Delay(5000);
                throw;
            }
        }
    }
}