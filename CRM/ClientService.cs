using System;
using System.Threading;
using System.Threading.Tasks;
using CrmLibrary.Models;

namespace CRM
{
    public static class ClientService
    {
        /// <summary>
        /// creates instance of client and inserts it to db by calling a different method
        /// </summary>
        /// <returns>instance of client</returns>
        private static async Task<ClientModel> CreateClientAsync()
        {
            Utils.RequiredWriter();
            try
            {
                var client = new ClientModel()
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
                client.Id = await DatabaseService.InsertIntoClientsAsync(client);
                Utils.CreationNotify(client.Id);
                await Task.Delay(1000);
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creating client model error - {ex.Message}");
            }
            return null;
        }
        
        /// <summary>
        /// creates instance of form and inserts it to db by calling a different method
        /// </summary>
        /// <returns>instance of form</returns>
        private static async Task<FormModel> CreateFormAsync(int clientId)
        {
            Utils.RequiredWriter();
            try
            {
                var form = new FormModel()
                {
                    ClientId = clientId,
                    Income = decimal.Parse(Utils.ConsoleWriteWithResult("Income *: ")),
                    CreditHistory = int.Parse(Utils.ConsoleWriteWithResult("Number of closed credits *: ")),
                    Defaults = int.Parse(Utils.ConsoleWriteWithResult("Number of defaults *: ")),
                };
                form.Id = await DatabaseService.InsertIntoFormsAsync(form);
                Utils.CreationNotify(form.Id);
                await Task.Delay(1000);
                return form;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creating form model error - {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// creates instance of credit application and inserts it to db by calling a different method
        /// </summary>
        /// <returns>instance of credit application</returns>
        private static async Task<CreditApplicationsModel> CreateCreditAppAsync(int clientId)
        { 
            Utils.RequiredWriter();
            try
            {
                var creditApp = new CreditApplicationsModel()
                {
                    ClientId = clientId,
                    Purpose = Utils.ConsoleWriteWithResult("" +
                                                           "Credit purpose *:\n" +
                                                           "1. Household appliances\n" +
                                                           "2. Renovation\n" +
                                                           "3. Phone\n" +
                                                           "4. Other\n" +
                                                           "Type 1, 2, 3, or 3: "
                                                           ),
                    Amount = decimal.Parse(Utils.ConsoleWriteWithResult("Credit amount *: ")),
                    Period = int.Parse(Utils.ConsoleWriteWithResult("Credit period *: ")),
                    IsApproved = false
                };
                creditApp.Id = await DatabaseService.InsertIntoApplicationsAsync(creditApp);
                Utils.CreationNotify(creditApp.Id);
                await Task.Delay(1000);
                return creditApp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creating credit application model error - {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// calculates client score according to input data
        /// </summary>
        /// <param name="client">instance of client model</param>
        /// <param name="form">instance of form model</param>
        /// <param name="creditApp">instance of credit application model</param>
        /// <returns>score</returns>
        private static int CalculateClientScore(ClientModel client, FormModel form,
            CreditApplicationsModel creditApp)
        {
            int score = 0;
            //checking gender
            score += client.Gender.Equals("F") ? 2 : 1;

            //checking marital status
            score += client.MaritalStatus switch
            {
                "1" => 1,
                "2" => 2,
                "3" => 1,
                "4" => 2,
                _ => 0
            };

            //checking age
            var yearOfBirth = client.DateOfBirth.Year;
            var yearNow = DateTime.Now.Year;
            var age = yearNow - yearOfBirth;
            score += age switch
            {
                <= 25 => 0,
                > 25 and <= 35 => 1,
                > 35 and <= 62 => 2,
                > 62 => 1,
            };

            //checking citizenship
            score += client.Citizenship.Equals("1") ? 1 : 0; 
            
            //checking credit sum from income amount
            var creditPerIncome = creditApp.Amount * 100 / form.Income;
            score += creditPerIncome switch
            {
                < 80m => 4,
                >= 80m and < 150m => 3,
                >= 150m and < 250m => 2,
                >= 250m => 1
            };

            //checking credit history
            score += form.CreditHistory switch
            {
                <= 0 => -1,
                > 0 and <= 2 => 1,
                >= 3 => 2
            };

            //checking defaults
            score += form.Defaults switch
            {
                <= 3 => 0,
                <= 4 => -1,
                >= 5 and <= 7 => -2,
                > 7 => -3
            };

            //checking credit purpose
            score += creditApp.Purpose switch
            {
                "1" => 2,
                "2" => 1,
                "3" => 0,
                _ => -1
            };

            //checking credit period
            score++;
            Console.WriteLine(score);
            return score;
        }
        
        /// <summary>
        /// creates instance of client, inserts it to db and updates isApproved field of credit app by calling other methods
        /// </summary>
        /// <returns>instance of client</returns>
        private static async Task<PaymentsSchedulesModel> CreatePaymentsSchedulesModelAsync(int score, CreditApplicationsModel app)
        {
            try
            {
                if (score > 11)
                {
                    await DatabaseService.UpdateApprovedInAppAsync(app);
                    app.IsApproved = true;
                    var paymentSchedule = new PaymentsSchedulesModel();
                    paymentSchedule.ClientID = app.ClientId;
                    paymentSchedule.ApplicationId = app.Id;
                    paymentSchedule.MonthlyPayment = app.Amount / app.Period;
                    paymentSchedule.StartDate = DateTime.Now;
                    paymentSchedule.EndDate = DateTime.Now.AddDays(app.Period * 30);
                    paymentSchedule.Id = await DatabaseService.InsertIntoPaymentsSchedulesAsync(paymentSchedule);
                    return paymentSchedule;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creating payment schedules model error - {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// creates instances of client model, form model, credit application model, calculates and evaluates score, creates payment schedule
        /// </summary>
        public static async Task CreditProcess()
        {
            try
            {
                var client = await CreateClientAsync();
                var form = await CreateFormAsync(client.Id);
                var creditApp = await CreateCreditAppAsync(client.Id);
                int score = CalculateClientScore(client, form, creditApp);
                Console.Clear();
                if (score > 11)
                {
                    var result = await CreatePaymentsSchedulesModelAsync(score, creditApp);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Congratulations, your credit application was approved. " +
                                      $"Below see the payment schedule");
                    Console.ResetColor();
                    for (int i = 1; i <= creditApp.Period; i++)
                    {
                        Console.WriteLine($"{result.StartDate.Month} - {result.MonthlyPayment}");
                        result.StartDate = result.StartDate.AddMonths(i);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unfortunately, your credit application was rejected");
                    Console.ResetColor();

                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Credit processing error - {ex.Message}");
            }
        }
    }
}