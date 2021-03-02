namespace CrmLibrary.Models
{
    public class FormModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public decimal Income { get; set; }
        public int CreditHistory { get; set; }
        public int Defaults { get; set; }
    }
}