using System.Linq;
using System.Web.Mvc;
using ContactBook.Core.Models;
using ContactBook.Data.Data;
using ContactBook.Core.Repository;

namespace ContactBook.MVC.Controllers
{
    public class CountryController : Controller
    {
        private IGenericRepo ClientDb;

        public CountryController()
        {
            ClientDb = new EFClientDb();
        }

        public CountryController(IGenericRepo clientDb)
        {
            ClientDb = clientDb;
        }

        // GET: /Country/
        public ActionResult Index()
        {
            return View(ClientDb.Query<Country>().ToList());
        }

        // GET: /Country/Details/5
        public ActionResult Details(int id)
        {
            
            Country country = ClientDb.GetById<Country>(id);
            
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // GET: /Country/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CountryName")] Country country)
        {
            if (ModelState.IsValid)
            {
                ClientDb.Add<Country>(country);
                ClientDb.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error Saving Record. Please try again...");
            }

            return View(country);
        }

        // GET: /Country/Edit/5
        public ActionResult Edit(int id)
        {
            Country country = ClientDb.GetById<Country>(id);
            
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,CountryName")] Country country)
        {
            if (ModelState.IsValid)
            {
                ClientDb.Update<Country>(country);
                ClientDb.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error Saving Record. Please try again...");
            }
            return View(country);
            
        }

        // GET: /Country/Delete/5
        public ActionResult Delete(int id)
        {
            Country country = ClientDb.GetById<Country>(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: /Country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Country country)
        {
            if (ModelState.IsValid)
            {
                ClientDb.Delete<Country>(country.Id);
                ClientDb.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Unable to delete record.");
            }

            return View(country);
        }
    }
}
