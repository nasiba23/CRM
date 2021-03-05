using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CrmLibrary.Models;

namespace CRM
{
    //methods for client registration
    
    /// <summary>
    /// database operations
    /// </summary>
    public static class DatabaseService
    {
        private const string ConnectionString = @"Data source=NASIBANOSIROVA\SQLEXPRESS; initial catalog=crmdb; integrated security=true";
        private static SqlConnection GetNewSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// inserts client into db
        /// </summary>
        /// <param name="client">instance of client model</param>
        /// <returns>client id</returns>
        public static async Task<int> InsertIntoClientsAsync(ClientModel client)
        {
            int clientId = 0;
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionAddClient = $"" +
                                                $" INSERT INTO CLIENTS (login , password, document_number, name, surname, fathers_name, date_of_birth, gender, citizenship, marital_status, taxpayer_id, address ) " +
                                                $" VALUES ('{client.Login}', '{client.Password}', '{client.DocumentNumber}', '{client.Name}', '{client.Surname}', '{client.FathersName}', '{client.DateOfBirth}', '{client.Gender}', {client.Citizenship}, '{client.MaritalStatus}', '{client.TaxpayerId}', '{client.Address}'); " +
                                                $" SELECT CAST(scope_identity() AS int) ";
                SqlCommand clientAddCommand = new (sqlExpressionAddClient, connection);
                connection.Open();
                var id = await clientAddCommand.ExecuteScalarAsync();
                clientId = (int)id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Inserting into clients error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return clientId;
        }
        
        /// <summary>
        /// inserts client into db
        /// </summary>
        /// <param name="form">instance of form model</param>
        /// <returns>form id</returns>
        public static async Task<int> InsertIntoFormsAsync(FormModel form)
        {
            int formId = 0;
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionAddForm = $"" +
                                                $" INSERT INTO FORMS (client_id, income, credit_history, defaults) " +
                                                $" VALUES ({form.ClientId}, {form.Income}, {form.CreditHistory}, {form.Defaults}); " +
                                                $" SELECT CAST(scope_identity() AS int) ";
                SqlCommand formAddCommand = new (sqlExpressionAddForm, connection);
                connection.Open();
                var id = await formAddCommand.ExecuteScalarAsync();
                formId = (int)id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Inserting into forms error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return formId;
        }
        
        /// <summary>
        /// inserts credit application into db
        /// </summary>
        /// <param name="application">instance of credit application</param>
        /// <returns>credit application id</returns>
        public static async Task<int> InsertIntoApplicationsAsync(CreditApplicationsModel application)
        {
            int applicationId = 0;
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionAddForm = $"" +
                                              $" INSERT INTO CREDIT_APPLICATIONS (client_id, purpose, amount, period, is_approved) " +
                                              $" VALUES ({application.ClientId}, '{application.Purpose}', {application.Amount}, {application.Period}, 0); " +
                                              $" SELECT CAST(scope_identity() AS int) ";
                SqlCommand applicationAddCommand = new (sqlExpressionAddForm, connection);
                connection.Open();
                var id = await applicationAddCommand.ExecuteScalarAsync();
                applicationId = (int)id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Inserting into applications error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return applicationId;
        }
        
        /// <summary>
        /// updates isApproved field in credit applications table
        /// </summary>
        /// <param name="creditApp">instance of  credit application</param>
        /// <returns>Task</returns>
        public static async Task UpdateApprovedInAppAsync(CreditApplicationsModel creditApp)
        {
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionUpdateApp = $"UPDATE CREDIT_APPLICATIONS SET is_approved = 1 WHERE id = {creditApp.Id} AND client_id = {creditApp.ClientId}; ";
                SqlCommand applicationUpdateCommand = new (sqlExpressionUpdateApp, connection);
                connection.Open();
                await applicationUpdateCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Updating isApproved in credit apps error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
        
        /// <summary>
        /// inserts payments schedules into db
        /// </summary>
        /// <param name="paymentsSchedule">instance of payments schedules</param>
        /// <returns></returns>
        public static async Task<int> InsertIntoPaymentsSchedulesAsync(PaymentsSchedulesModel paymentsSchedule)
        {
            int scheduleId = 0;
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionAddPaymentsSchedules = $"" +
                                              $" INSERT INTO PAYMENTS_SCHEDULES (client_id, application_id, monthly_payment, start_date, end_date) " +
                                              $" VALUES ({paymentsSchedule.ClientID}, {paymentsSchedule.ApplicationId}, {paymentsSchedule.MonthlyPayment}, '{paymentsSchedule.StartDate}', '{paymentsSchedule.EndDate}'); " +
                                              $" SELECT CAST(scope_identity() AS int) ";
                SqlCommand paymentsSchedulesAddCommand = new (sqlExpressionAddPaymentsSchedules, connection);
                connection.Open();
                var id = await paymentsSchedulesAddCommand.ExecuteScalarAsync();
                scheduleId = (int)id;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Inserting into payments schedules error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return scheduleId;
        }

        //methods for admin login
        public static async Task<AdminModel> AuthorizationCheck(string login, string password)
        {
            AdminModel admin = new AdminModel();
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionCheckAuthorization = $"" +
                                                         $" SELECT * FROM ADMINS WHERE login = '{login}' and password = '{password}' ";
                SqlCommand authorizationCheckCommand = new (sqlExpressionCheckAuthorization, connection);
                connection.Open();
                SqlDataReader reader = await authorizationCheckCommand.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        admin.Id = (int)reader.GetValue(0);
                        admin.Name = (string)reader.GetValue(4);
                        return admin;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Checking authorization error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return null;
        }
        
        /// <summary>
        /// inserts admin into db
        /// </summary>
        /// <param name="admin">instance of admin model</param>
        /// <returns>client id</returns>
        public static async Task InsertIntoAdminsAsync(AdminModel admin)
        {
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionAddAdmin = $"" +
                                                $" INSERT INTO ADMINS (login , password, document_number, name, surname, fathers_name, date_of_birth, gender, citizenship, marital_status, taxpayer_id, address ) " +
                                                $" VALUES ('{admin.Login}', '{admin.Password}', '{admin.DocumentNumber}', '{admin.Name}', '{admin.Surname}', '{admin.FathersName}', '{admin.DateOfBirth}', '{admin.Gender}', {admin.Citizenship}, '{admin.MaritalStatus}', '{admin.TaxpayerId}', '{admin.Address}'); ";
                SqlCommand adminAddCommand = new (sqlExpressionAddAdmin, connection);
                connection.Open();
                await adminAddCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Inserting into admins error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// updates password of admin in db
        /// </summary>
        /// <param name="id">admin's id</param>
        /// <param name="currentPassword">current admin's password</param>
        /// <param name="newPassword">new password</param>
        /// <returns>number of rows affected</returns>
        public static async Task<int> UpdatePasswordInDbAsync(int id, string currentPassword, string newPassword)
        {
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionPasswordUpdate = $"" +
                                                     $" UPDATE ADMINS   " +
                                                     $" SET password = '{newPassword}' " +
                                                     $" WHERE id = {id} and password = '{currentPassword}' ";
                SqlCommand passwordUpdateCommand = new (sqlExpressionPasswordUpdate, connection);
                connection.Open();
                var result = await passwordUpdateCommand.ExecuteNonQueryAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Updating password error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return 0;
        }

        /// <summary>
        /// selects all credit applications from db and returns them in a list
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CreditApplicationsModel>> SelectAllApplicationsFromDb()
        {
            SqlConnection connection = GetNewSqlConnection();
            var listApps = new List<CreditApplicationsModel>();
            try
            {
                connection.Open();
                string sqlExpressionSelectApps = $"SELECT * FROM credit_applications ";
                SqlCommand selectAppsCommand = new (sqlExpressionSelectApps, connection);
                SqlDataReader reader = await selectAppsCommand.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var creditApp = new CreditApplicationsModel();
                        creditApp.Id = (int)reader.GetValue(0);
                        creditApp.ClientId = (int)reader.GetValue(1);
                        creditApp.Purpose = (string)reader.GetValue(2) switch
                        {
                            "1" => "Home appliances",
                            "2" => "Renovation",
                            "3" => "Phone",
                            "4" => "Other",
                            _ => ""
                        };
                        creditApp.Amount = (decimal)reader.GetValue(3);
                        creditApp.Period = (int)reader.GetValue(4);
                        creditApp.IsApproved = (bool) reader.GetValue(5);
                        listApps.Add(creditApp);
                    }
                    return listApps;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selecting all credit applications from db error {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return null;
        }
    }
}