namespace URabbitMQ.Shared.Dtos
{
    public class Product
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int AvailableStock { get; set; }
    }
}
