using System.Data.Entity;

namespace FirstAppRadiuk.Models
{
    public class GroceryContext : DbContext
    {
        // Таблиця продуктів
        public DbSet<Product> Products { get; set; }
        // Таблиця замовлень
        public DbSet<Order> Orders { get; set; }
    }
}