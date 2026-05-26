namespace FirstAppRadiuk.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Weight { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int? CategoryId { get; set; }
        public Category CategoryNav { get; set; }
    }
}