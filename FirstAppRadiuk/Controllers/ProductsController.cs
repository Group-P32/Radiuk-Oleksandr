//Якщо файл відкриється умовно на /Products/Buy, то прошу перейти на  localhost:55627/Products 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FirstAppRadiuk.Models;
using FirstAppRadiuk.Helpers;

namespace FirstAppRadiuk.Controllers
{
    public class ProductsController : Controller
    {
        private GroceryContext db = new GroceryContext();

        // Список продуктів з фільтрацією та пагінацією
        // page - номер сторінки (за замовчуванням 1)
        // category - ID категорії для фільтру
        // manufacturer - виробник для фільтру
        public ActionResult Index(int page = 1, int? category = null, string manufacturer = null)
        {
            int pageSize = 3; // кількість продуктів на одній сторінці

            // Отримуємо всі продукти з бази
            IQueryable<Product> products = db.Products;

            // Фільтр по категорії
            if (category != null && category != 0)
            {
                products = products.Where(p => p.CategoryId == category);
            }
            // Фільтр по виробнику
            if (!String.IsNullOrEmpty(manufacturer) && !manufacturer.Equals("Всі"))
            {
                products = products.Where(p => p.Manufacturer == manufacturer);
            }

            // Рахуємо скільки всього продуктів після фільтру
            int totalItems = products.Count();

            // Беремо тільки продукти для поточної сторінки
            List<Product> productsPerPage = products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Список категорій для фільтру
            List<Category> categories = db.Categories.ToList();
            categories.Insert(0, new Category { Name = "Всі", Id = 0 });

            // Список виробників для фільтру (унікальні)
            List<string> manufacturers = db.Products
                .Select(p => p.Manufacturer)
                .Distinct()
                .ToList();
            manufacturers.Insert(0, "Всі");

            // Формуємо ViewModel з усіма даними
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = productsPerPage,
                Categories = new SelectList(categories, "Id", "Name"),
                Manufacturers = new SelectList(manufacturers),
                PageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                },
                // Зберігаємо фільтри щоб кнопки пагінації їх не скидали
                SelectedCategory = category,
                SelectedManufacturer = manufacturer
            };

            return View(viewModel);
        }

        // Форма додавання — GET
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // Форма додавання — POST
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Видалення продукту — GET (підтвердження)
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

        // Видалення продукту — POST
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Buy(int? id)
        {
            if (id == null) return HttpNotFound();
            ViewBag.ProductId = id;
            return View();
        }

        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            db.Purchases.Add(purchase);
            db.SaveChanges();
            return "Дякуємо, " + purchase.Person + ", за покупку!";
        }

        // Редагування - GET
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();
            Product product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // Редагування - POST
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}