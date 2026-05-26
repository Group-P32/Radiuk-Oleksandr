//Якщо файл відкриється умовно на /Products/Buy, то прошу перейти на  localhost:55627/Products 
using System.Linq;
using System;
using System.Web.Mvc;
using FirstAppRadiuk.Models;

namespace FirstAppRadiuk.Controllers
{
    public class ProductsController : Controller
    {
        private GroceryContext db = new GroceryContext();

        // Список усіх продуктів
        public ActionResult Index()
        {
            return View(db.Products.ToList());
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