using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperSecret.DAL;
using SuperSecret.Models;

namespace SuperSecret.Controllers
{
    [Authorize]
    public class TypeOfCrimesController : Controller
    {
        private SuperSecretContext db = new SuperSecretContext();

        // GET: TypeOfCrimes
        public ActionResult Index()
        {
            return View(db.TypeOfCrimes.ToList());
        }

        // GET: TypeOfCrimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfCrime typeOfCrime = db.TypeOfCrimes.Find(id);
            if (typeOfCrime == null)
            {
                return HttpNotFound();
            }
            return View(typeOfCrime);
        }

        // GET: TypeOfCrimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeOfCrimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CrimeTypeName")] TypeOfCrime typeOfCrime)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.TypeOfCrimes.Add(typeOfCrime);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Kan ikke lagre endringer. Prøv igjen, om problemet fortsetter kontakt din system administrator.");
            }

            return View(typeOfCrime);
        }

        // GET: TypeOfCrimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfCrime typeOfCrime = db.TypeOfCrimes.Find(id);
            if (typeOfCrime == null)
            {
                return HttpNotFound();
            }
            return View(typeOfCrime);
        }

        // POST: TypeOfCrimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeOfCrimeId,CrimeTypeName")] TypeOfCrime typeOfCrime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeOfCrime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeOfCrime);
        }

        // GET: TypeOfCrimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfCrime typeOfCrime = db.TypeOfCrimes.Find(id);
            if (typeOfCrime == null)
            {
                return HttpNotFound();
            }
            return View(typeOfCrime);
        }

        // POST: TypeOfCrimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeOfCrime typeOfCrime = db.TypeOfCrimes.Find(id);
            db.TypeOfCrimes.Remove(typeOfCrime);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
