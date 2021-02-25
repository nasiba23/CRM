﻿namespace CrmLibrary.ViewModels
{
    public class CreditApplicationsViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Purpose { get; set; }
        public decimal Amount { get; set; }
        public int Period { get; set; }
        public bool IsApproved { get; set; }
    }
}