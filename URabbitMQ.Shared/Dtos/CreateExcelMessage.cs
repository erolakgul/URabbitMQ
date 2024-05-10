namespace URabbitMQ.Shared.Dtos
{
    public class CreateExcelMessage
    {
        public string? UserID { get; set; }
        public int FileId { get; set; }
        // worker service ile db den çekeceğiz verileri, taşıma olmayacak
        //public List<Product>? Products { get; set; }
    }
}
