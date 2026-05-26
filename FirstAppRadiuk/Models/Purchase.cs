//Якщо файл відкриється умовно на /Products/Buy, то прошу перейти на  localhost:55627/Products 
using System;
namespace FirstAppRadiuk.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Person { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
    }
}