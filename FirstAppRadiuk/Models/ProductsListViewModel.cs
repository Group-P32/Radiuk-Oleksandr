using System.Collections.Generic;
using System.Web.Mvc;

namespace FirstAppRadiuk.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Manufacturers { get; set; }
        // Інформація для пагінації
        public PageInfo PageInfo { get; set; }
        // Зберігаємо поточні фільтри щоб кнопки сторінок їх не скидали
        public int? SelectedCategory { get; set; }
        public string SelectedManufacturer { get; set; }
    }
}