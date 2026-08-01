namespace ExpenseLayeredApi.DTO
{
    public class IncomeUpdateDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Title { get; set; }
        public DateTime IncomeDate { get; set; }
    }
}
