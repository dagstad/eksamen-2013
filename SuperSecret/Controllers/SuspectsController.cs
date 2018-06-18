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
using System.IO;
using PagedList;
namespace SuperSecret.Controllers
{
    [Authorize]
    public class SuspectsController : Controller
    {
        private SuperSecretContext db = new SuperSecretContext();

        // GET: Suspects
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AliasSortParm = String.IsNullOrEmpty(sortOrder) ? "alias_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var suspects = from s in db.Suspects
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                suspects = suspects.Where(s => s.Name.Contains(searchString));
            }



            switch (sortOrder)
            {
                case "name_desc":
                    suspects = suspects.OrderByDescending(s => s.Name);
                    break;
                case "alias_desc":
                    suspects = suspects.OrderByDescending(s => s.Alias);
                    break;
                default:
                    suspects = suspects.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(suspects.ToPagedList(pageNumber, pageSize));
        }


        // GET: Suspects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suspect suspect = db.Suspects
                .Include(s => s.FilePaths).SingleOrDefault(s => s.SuspectId == id);

            if (suspect == null)
            {
                return HttpNotFound();
            }
            return View(suspect);
        }

        // GET: Suspects/Create
        public ActionResult Create(int? page)
        {
            var suspect = new Suspect();
            suspect.Crimes = new List<Crime>();
            PopulateCrimesData(suspect);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");


            return View();
        }

        // POST: Suspects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryId,PictureId,Name,Alias,Age,Crime_CrimeId")] Suspect suspect, string[] selectedCrimes, HttpPostedFileBase upload)
        {

            if (selectedCrimes != null)
            {
                suspect.Crimes = new List<Crime>();
                foreach (var crime in selectedCrimes)
                {
                    var crimeToAdd = db.Crimes.Find(int.Parse(crime));
                    suspect.Crimes.Add(crimeToAdd);
                }
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var photo = new FilePath
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            Filetype = FileType.Photo

                        };
                        suspect.FilePaths = new List<FilePath>();
                        suspect.FilePaths.Add(photo);
                        upload.SaveAs(Path.Combine(Server.MapPath("~/images"), photo.FileName));
                    }
                    db.Suspects.Add(suspect);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Kan ikke lagre endringer. Prøv igjen, om problemet fortsetter kontakt din system administrator.");
            }
            PopulateCrimesData(suspect);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", suspect.CountryId);
            return View(suspect);
        }

        // GET: Suspects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Suspect suspect = db.Suspects

                .Include(i => i.Crimes)
                .Where(i => i.SuspectId == id)
                .Single();

            PopulateCrimesData(suspect);



            if (suspect == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", suspect.CountryId);
            return View(suspect);
        }

        private void PopulateCrimesData(Suspect suspect)
        {
            var allCrimes = db.Crimes;
            var suspectCrimes = new HashSet<int>(suspect.Crimes.Select(c => c.CrimeId));
            var viewModel = new List<AssignCrimeData>();
            foreach (var crimes in allCrimes)
            {
                viewModel.Add(new AssignCrimeData
                {
                    CrimeId = crimes.CrimeId,
                    TypeOfCrimeId = crimes.TypeOfCrimeId,
                    Description = crimes.Description,
                    Date = crimes.Date,
                    TypeOfCrime = crimes.TypeOfCrime,
                    Assigned = suspectCrimes.Contains(crimes.CrimeId)

                });
            }
            ViewBag.Crimes = viewModel;


        }

        // POST: Suspects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCrime, HttpPostedFileBase upload)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var suspectToUpdate = db.Suspects
                .Include(i => i.Crimes)
                .Where(i => i.SuspectId == id)
                .Single();

            if (TryUpdateModel(suspectToUpdate, "", new string[] { "CountryId", "PictureId", "Name", "Alias", "Age" }))
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        if (suspectToUpdate.FilePaths.Any(p => p.Filetype == FileType.Photo))
                        {
                            db.FilePaths.Remove(suspectToUpdate.FilePaths.First(p => p.Filetype == FileType.Photo));
                        }
                        var photo = new FilePath
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            Filetype = FileType.Photo,

                        };

                        suspectToUpdate.FilePaths = new List<FilePath> { photo };
                        upload.SaveAs(Path.Combine(Server.MapPath("~/images"), photo.FileName));
                    }


                    UpdateCrimesOnSuspect(selectedCrime, suspectToUpdate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Kan ikke lagre endringer. Prøv igjen, om problemet fortsetter kontakt din system administrator.");

                }
            }
            PopulateCrimesData(suspectToUpdate);
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", suspectToUpdate.CountryId);
            return View(suspectToUpdate);
        }

        private void UpdateCrimesOnSuspect(string[] selectedCrime, Suspect suspectToUpdate)
        {
            if (selectedCrime == null)
            {
                suspectToUpdate.Crimes = new List<Crime>();
                return;
            }

            var selectedSuspectHS = new HashSet<string>(selectedCrime);
            var suspectCrimes = new HashSet<int>
                (suspectToUpdate.Crimes.Select(s => s.CrimeId));
            foreach (var crime in db.Crimes)
            {
                if (selectedSuspectHS.Contains(crime.CrimeId.ToString()))
                {
                    if (!suspectCrimes.Contains(crime.CrimeId))
                    {
                        suspectToUpdate.Crimes.Add(crime);
                    }
                }
                else
                {
                    if (suspectCrimes.Contains(crime.CrimeId))
                    {
                        suspectToUpdate.Crimes.Remove(crime);
                    }
                }
            }

        }

        //public ActionResult Edit([Bind(Include = "SuspectId,CountryId,PictureId,Name,Alias,Age")] Suspect suspect)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(suspect).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", suspect.CountryId);
        //    return View(suspect);
        //}

        // GET: Suspects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suspect suspect = db.Suspects.Find(id);
            if (suspect == null)
            {
                return HttpNotFound();
            }
            return View(suspect);
        }

        // POST: Suspects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suspect suspect = db.Suspects
                .Include(i => i.Crimes)
                .Where(i => i.SuspectId == id)
                .Single();
            suspect.Crimes = null;
            db.Suspects.Remove(suspect);
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
