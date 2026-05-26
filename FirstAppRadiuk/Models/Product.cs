//Якщо файл відкриється умовно на /Products/Buy, то прошу перейти на  localhost:55627/Products 
namespace FirstAppRadiuk.Models
{
    public class Product
    {
        // ID продукту
        public int Id { get; set; }
        // Назва продукту
        public string Name { get; set; }
        // Категорія (молочні, м'ясні, овочі тощо)
        public string Category { get; set; }
        // Виробник
        public string Manufacturer { get; set; }
        // Вага/об'єм (наприклад: "1 кг", "0.5 л")
        public string Weight { get; set; }
        // Ціна (грн)
        public decimal Price { get; set; }
        // Кількість на складі
        public int Quantity { get; set; }
    }
}