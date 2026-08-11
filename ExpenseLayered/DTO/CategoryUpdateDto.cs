namespace ExpenseLayeredApi.DTO
{
    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
    }
}
