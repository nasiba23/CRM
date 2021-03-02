using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CrmLibrary.Models;

namespace CRM
{
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

        private static int _clientId;
        private static int _formId;
        private static int _applicationId;
        private static int _scheduleId;

        /// <summary>
        /// inserts client into db
        /// </summary>
        /// <param name="client">instance of client model</param>
        /// <returns>client id</returns>
        public static async Task<int> InsertIntoClientsAsync(ClientModel client)
        {
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionAddClient = $"" +
                                                $" INSERT INTO CLIENTS (login , password, document_number, name, surname, fathers_name, date_of_birth, gender, citizenship, marital_status, taxpayer_id, address ) " +
                                                $" VALUES ('{client.Login}', '{client.Password}', '{client.DocumentNumber}', '{client.Name}', '{client.Surname}', '{client.FathersName}', '{client.DateOfBirth}', '{client.Gender}', {client.Citizenship}, '{client.MaritalStatus}', '{client.TaxpayerId}', '{client.Address}'); " +
                                                $" SELECT CAST(scope_identity() AS int) ";
                SqlCommand clientAddCommand = new SqlCommand(sqlExpressionAddClient, connection);
                connection.Open();
                var id = await clientAddCommand.ExecuteScalarAsync();
                _clientId = (int)id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Inserting into clients error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return _clientId;
        }
        
        /// <summary>
        /// inserts client into db
        /// </summary>
        /// <param name="form">instance of form model</param>
        /// <returns>form id</returns>
        public static async Task<int> InsertIntoFormsAsync(FormModel form)
        {
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionAddForm = $"" +
                                                $" INSERT INTO FORMS (client_id, income, credit_history, defaults) " +
                                                $" VALUES ({form.ClientId}, {form.Income}, {form.CreditHistory}, {form.Defaults}); " +
                                                $" SELECT CAST(scope_identity() AS int) ";
                SqlCommand formAddCommand = new SqlCommand(sqlExpressionAddForm, connection);
                connection.Open();
                var id = await formAddCommand.ExecuteScalarAsync();
                _formId = (int)id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Inserting into forms error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return _formId;
        }
        
        /// <summary>
        /// inserts credit application into db
        /// </summary>
        /// <param name="application">instance of credit application</param>
        /// <returns>credit application id</returns>
        public static async Task<int> InsertIntoApplicationsAsync(CreditApplicationsModel application)
        {
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionAddForm = $"" +
                                              $" INSERT INTO CREDIT_APPLICATIONS (client_id, purpose, amount, period, is_approved) " +
                                              $" VALUES ({application.ClientId}, '{application.Purpose}', {application.Amount}, {application.Period}, 0); " +
                                              $" SELECT CAST(scope_identity() AS int) ";
                SqlCommand applicationAddCommand = new SqlCommand(sqlExpressionAddForm, connection);
                connection.Open();
                var id = await applicationAddCommand.ExecuteScalarAsync();
                _applicationId = (int)id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Inserting into applications error - {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return _applicationId;
        }
        
        /// <summary>
        /// updates isApproved field in credit applications table
        /// </summary>
        /// <param name="creditApp">instance of  credit application</param>
        public static async Task UpdateApprovedInAppAsync(CreditApplicationsModel creditApp)
        {
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionUpdateApp = $"UPDATE CREDIT_APPLICATIONS SET is_approved = 1 WHERE id = {creditApp.Id} AND client_id = {creditApp.ClientId}; ";
                SqlCommand applicationUpdateCommand = new SqlCommand(sqlExpressionUpdateApp, connection);
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
            SqlConnection connection = GetNewSqlConnection();
            try
            {
                string sqlExpressionAddPaymentsSchedules = $"" +
                                              $" INSERT INTO PAYMENTS_SCHEDULES (client_id, application_id, monthly_payment, start_date, end_date) " +
                                              $" VALUES ({paymentsSchedule.ClientID}, {paymentsSchedule.ApplicationId}, {paymentsSchedule.MonthlyPayment}, '{paymentsSchedule.StartDate}', '{paymentsSchedule.EndDate}'); " +
                                              $" SELECT CAST(scope_identity() AS int) ";
                SqlCommand paymentsSchedulesAddCommand = new SqlCommand(sqlExpressionAddPaymentsSchedules, connection);
                connection.Open();
                var id = await paymentsSchedulesAddCommand.ExecuteScalarAsync();
                _scheduleId = (int)id;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Inserting into payments schedules error - {ex.Message}");;
            }
            finally
            {
                connection.Close();
            }
            return _scheduleId;
        }

    }
}