using System.Linq;
using System.Web.Mvc;
using FirstAppRadiuk.Models;

namespace FirstAppRadiuk.Controllers
{
    public class CategoriesController : Controller
    {
        private GroceryContext db = new GroceryContext();

        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return HttpNotFound();
            Category category = db.Categories.Find(id);
            if (category == null) return HttpNotFound();
            category.Products = db.Products.Where(p => p.CategoryId == category.Id).ToList();
            return View(category);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null) return HttpNotFound();
            Category category = db.Categories.Find(id);
            if (category == null) return HttpNotFound();
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}