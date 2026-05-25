using System.Linq;
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
    }
}