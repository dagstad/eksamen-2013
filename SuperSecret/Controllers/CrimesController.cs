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
using SuperSecret.ViewModels;
using System.Data.Entity.Infrastructure;
using PagedList;

namespace SuperSecret.Controllers
{
    [Authorize]
    public class CrimesController : Controller
    {
        private SuperSecretContext db = new SuperSecretContext();

        // GET: Crimes
        public ViewResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DescriptionSortParm = String.IsNullOrEmpty(sortOrder) ? "description_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var crimes = db.Crimes.Include(c => c.Suspects).Include(c => c.TypeOfCrime);

            switch (sortOrder)
            {

                case "description_desc":
                    crimes = crimes.OrderByDescending(s => s.Description);
                    break;
                case "Date":
                    crimes = crimes.OrderBy(s => s.Date);
                    break;
                default:
                    crimes = crimes.OrderBy(s => s.Description);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View(crimes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Crimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crime crime = db.Crimes.Find(id);
            if (crime == null)
            {
                return HttpNotFound();
            }
            return View(crime);
        }

        // GET: Crimes/Create
        public ActionResult Create()
        {
            var crime = new Crime();
            crime.Suspects = new List<Suspect>();
            PopulateAssignedSuspects(crime);

            //ViewBag.SuspectId = new SelectList(db.Suspects, "SuspectId", "Name");
            ViewBag.TypeOfCrimeId = new SelectList(db.TypeOfCrimes, "TypeOfCrimeId", "CrimeTypeName");
            return View();
        }

        // POST: Crimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeOfCrimeId,SuspectId,Description,Date,Suspect_SuspectId, Suspect_SuspectId1")] Crime crime, string[] selectedSuspects)
        {
            if (selectedSuspects != null)
            {
                crime.Suspects = new List<Suspect>();
                foreach (var suspect in selectedSuspects)
                {
                    var suspectToAdd = db.Suspects.Find(int.Parse(suspect));
                    crime.Suspects.Add(suspectToAdd);
                }
            }
            try
            {
                if (ModelState.IsValid)
                {
                    db.Crimes.Add(crime);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Kan ikke lagre endringer. Prøv igjen, om problemet fortsetter kontakt din system administrator.");
            }
            PopulateAssignedSuspects(crime);
            //ViewBag.SuspectId = new SelectList(db.Suspects, "SuspectId", "Name", crime.SuspectId);
            ViewBag.TypeOfCrimeId = new SelectList(db.TypeOfCrimes, "TypeOfCrimeId", "CrimeTypeName", crime.TypeOfCrimeId);
            return View(crime);
        }

        // GET: Crimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crime crime = db.Crimes
                .Include(i => i.Suspects)
                .Where(i => i.CrimeId == id)
                .Single();
            PopulateAssignedSuspects(crime);

            if (crime == null)
            {
                return HttpNotFound();
            }
            // ViewBag.SuspectId = new SelectList(db.Suspects, "SuspectId", "Name", crime.SuspectId);
            ViewBag.TypeOfCrimeId = new SelectList(db.TypeOfCrimes, "TypeOfCrimeId", "CrimeTypeName", crime.TypeOfCrimeId);
            return View(crime);
        }

        private void PopulateAssignedSuspects(Crime crime)
        {
            var allSuspects = db.Suspects;
            var crimesSuspects = new HashSet<int>(crime.Suspects.Select(s => s.SuspectId));
            var viewModel = new List<AssignSuspectData>();
            foreach (var suspect in allSuspects)
            {
                viewModel.Add(new AssignSuspectData
                    {
                        SuspectId = suspect.SuspectId,
                        Name = suspect.Name,
                        Age = suspect.Age,
                        Alias = suspect.Alias,
                        Country = suspect.Country,
                        Assigned = crimesSuspects.Contains(suspect.SuspectId)

                    }
                    );
            }
            ViewBag.Suspects = viewModel;
        }

        // POST: Crimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedSuspect)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var crimeToUpdate = db.Crimes
                .Include(i => i.Suspects)
                .Where(i => i.CrimeId == id)
                .Single();
            // var suspectToUpdate = db.Suspects.Include(i => i.Crimes).Where(i => i.SuspectId == crimeToUpdate.CrimeId).Single();

            if (TryUpdateModel(crimeToUpdate, "", new string[] { "TypeOfCrimeId", "SuspectId", "Description", "Date", "Suspect_SuspectId", "Suspect_SuspectId1" }))
            {
                try
                {
                    UpdateSuspectsOnCrime(selectedSuspect, crimeToUpdate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Kan ikke lagre endringer. Prøv igjen, om problemet fortsetter kontakt din system administrator.");

                }
            }
            //ViewBag.SuspectId = new SelectList(db.Suspects, "SuspectId", "Name", crimeToUpdate.SuspectId);
            ViewBag.TypeOfCrimeId = new SelectList(db.TypeOfCrimes, "TypeOfCrimeId", "CrimeTypeName", crimeToUpdate.TypeOfCrimeId);
            return View(crimeToUpdate);
        }

        private void UpdateSuspectsOnCrime(string[] selectedSuspect, Crime crimeToUpdate)
        {
            if (selectedSuspect == null)
            {
                crimeToUpdate.Suspects = new List<Suspect>();
                return;
            }

            var selectedCrimesHS = new HashSet<string>(selectedSuspect);
            var crimeSuspects = new HashSet<int>
                (crimeToUpdate.Suspects.Select(s => s.SuspectId));
            foreach (var suspect in db.Suspects)
            {
                if (selectedCrimesHS.Contains(suspect.SuspectId.ToString()))
                {
                    if (!crimeSuspects.Contains(suspect.SuspectId))
                    {
                        crimeToUpdate.Suspects.Add(suspect);

                    }
                }
                else
                {
                    if (crimeSuspects.Contains(suspect.SuspectId))
                    {
                        crimeToUpdate.Suspects.Remove(suspect);
                    }
                }
            }
        }


        // GET: Crimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crime crime = db.Crimes.Find(id);
            if (crime == null)
            {
                return HttpNotFound();
            }
            return View(crime);
        }

        // POST: Crimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Crime crime = db.Crimes
                .Include(i => i.Suspects)
                .Where(i => i.CrimeId == id)
                .Single();
            crime.Suspects = null;
            db.Crimes.Remove(crime);

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
