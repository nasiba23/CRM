using System;

namespace CrmLibrary.Models
{
    public class PaymentsSchedules
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public decimal MonthlyPayment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}