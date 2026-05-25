using System;

namespace FirstAppRadiuk.Models
{
    public class Order
    {
        // ID замовлення
        public int OrderId { get; set; }
        // Ім'я та прізвище покупця
        public string CustomerName { get; set; }
        // Номер телефону
        public string Phone { get; set; }
        // Адреса доставки
        public string Address { get; set; }
        // ID продукту
        public int ProductId { get; set; }
        // Кількість одиниць
        public int Count { get; set; }
        // Дата замовлення
        public DateTime Date { get; set; }
    }
}