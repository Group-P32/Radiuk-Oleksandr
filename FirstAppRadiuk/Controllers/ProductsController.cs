//Якщо файл відкриється умовно на /Products/Buy, то прошу перейти на  localhost:55627/Products 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FirstAppRadiuk.Models;

namespace FirstAppRadiuk.Controllers
{
    public class ProductsController : Controller
    {
        private GroceryContext db = new GroceryContext();

        // Список продуктів з фільтрацією
        // category - ID категорії для фільтру (0 або null = всі)
        // manufacturer - назва виробника для фільтру ("Всі" = всі)
        public ActionResult Index(int? category, string manufacturer)
        {
            // Отримуємо всі продукти з бази даних
            IQueryable<Product> products = db.Products;

            // Фільтр по категорії - якщо вибрано конкретну категорію
            if (category != null && category != 0)
            {
                products = products.Where(p => p.CategoryId == category);
            }

            // Фільтр по виробнику - якщо вибрано конкретного виробника
            if (!String.IsNullOrEmpty(manufacturer) && !manufacturer.Equals("Всі"))
            {
                products = products.Where(p => p.Manufacturer == manufacturer);
            }

            // Отримуємо список категорій і додаємо пункт "Всі" на початок
            List<Category> categories = db.Categories.ToList();
            categories.Insert(0, new Category { Name = "Всі", Id = 0 });

            // Отримуємо унікальних виробників і додаємо "Всі" на початок
            List<string> manufacturers = db.Products
                .Select(p => p.Manufacturer)
                .Distinct()
                .ToList();
            manufacturers.Insert(0, "Всі");

            // Формуємо ViewModel з продуктами і списками для фільтрів
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = products.ToList(),
                Categories = new SelectList(categories, "Id", "Name"),
                Manufacturers = new SelectList(manufacturers)
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
            if (id == null)
            {
                return HttpNotFound();
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
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