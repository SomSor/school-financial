namespace School.Financial.Models
{
    public class IncomeDetail : DbModelBase
    {
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public int TransactionId { get; set; }
    }
}
