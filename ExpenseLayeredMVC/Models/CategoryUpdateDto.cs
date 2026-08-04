namespace ExpenseLayeredMVC.Models
{
    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
}
