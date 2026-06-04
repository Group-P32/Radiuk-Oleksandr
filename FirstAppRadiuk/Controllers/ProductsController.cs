//Якщо файл відкриється умовно на /Products/Buy, то прошу перейти на localhost:55627/Products
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FirstAppRadiuk.Models;
using FirstAppRadiuk.Helpers;
using FirstAppRadiuk.Filters;
using System.Web;
namespace FirstAppRadiuk.Controllers
{
    [Culture]
    public class ProductsController : Controller
    {
        private GroceryContext db = new GroceryContext();

        public ActionResult Index(int page = 1, int? category = null, string manufacturer = null)
        {
            int pageSize = 3;
            IQueryable<Product> products = db.Products;

            if (category != null && category != 0)
                products = products.Where(p => p.CategoryId == category);

            if (!String.IsNullOrEmpty(manufacturer) && !manufacturer.Equals("Всі"))
                products = products.Where(p => p.Manufacturer == manufacturer);

            int totalItems = products.Count();
            List<Product> productsPerPage = products.OrderBy(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            List<Category> categories = db.Categories.ToList();
            categories.Insert(0, new Category { Name = "Всі", Id = 0 });

            List<string> manufacturers = db.Products.Select(p => p.Manufacturer).Distinct().ToList();
            manufacturers.Insert(0, "Всі");

            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = productsPerPage,
                Categories = new SelectList(categories, "Id", "Name"),
                Manufacturers = new SelectList(manufacturers),
                PageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems },
                SelectedCategory = category,
                SelectedManufacturer = manufacturer
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

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

        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

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

        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();
            Product product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

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

        // Зміна мови сайту — зберігає вибір у кукі
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            List<string> cultures = new List<string>() { "uk", "en" };
            if (!cultures.Contains(lang))
                lang = "uk";

            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }
    }
}