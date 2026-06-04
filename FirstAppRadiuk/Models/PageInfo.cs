using System;

namespace FirstAppRadiuk.Models
{
    public class PageInfo
    {
        // Номер поточної сторінки
        public int PageNumber { get; set; }
        // Кількість продуктів на одній сторінці
        public int PageSize { get; set; }
        // Загальна кількість продуктів (після фільтру)
        public int TotalItems { get; set; }
        // Загальна кількість сторінок — рахується автоматично
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
}