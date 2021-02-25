using System;

namespace CrmLibrary.Models
{
    public class ClientModel
    {
        public int Id { get; set;}
        public string Login { get; set; }
        public string Password { get; set; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FathersName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Citizenship { get; set; }
        public string MaritalStatus { get; set; }
        public string TaxpayerId { get; set; }
        public string Address { get; set; }
    }
}