//Якщо файл відкриється умовно на /Products/Buy, то прошу перейти на  localhost:55627/Products 
using System.Data.Entity;

namespace FirstAppRadiuk.Models
{
    public class GroceryContext : DbContext
    {
        // Таблиця продуктів
        public DbSet<Product> Products { get; set; }
        // Таблиця замовлень
        public DbSet<Order> Orders { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}