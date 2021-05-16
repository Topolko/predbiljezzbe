using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TopolkoSeminar.Models;

namespace TopolkoSeminar.Controllers
{
    [Authorize]
    public class PredbiljezbasController : Controller
    {
        private PredbiljezbedbEntities db = new PredbiljezbedbEntities();

        // GET: Predbiljezbas
        #region index
        public ActionResult Index(string sortOrder, string searchString, string SearchBy, string filter, string filterKat)
        {
            var predbiljezbas = db.Predbiljezbas.Include(p => p.Seminar);


            ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";
            ViewBag.DatumSortParm = sortOrder == "Datum" ? "Datum_desc" : "Datum";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "Status_desc" : "Status";
            ViewBag.ImeSortParm = sortOrder == "Ime" ? "Ime_desc" : "Ime";
            ViewBag.PrezimeSortParm = sortOrder == "Prezime" ? "Prezime_desc" : "Prezime";
            ViewBag.AdresaSortParm = sortOrder == "Adresa" ? "Adresa_desc" : "Adresa";

            predbiljezbas = from s in db.Predbiljezbas
                            select s;


            switch (sortOrder)
            {
                case "naziv_desc":
                    predbiljezbas = predbiljezbas.OrderByDescending(s => s.IdSeminar);
                    break;
                case "Datum":
                    predbiljezbas = predbiljezbas.OrderBy(s => s.Datum);
                    break;
                case "Datum_desc":
                    predbiljezbas = predbiljezbas.OrderByDescending(s => s.Datum);
                    break;
                case "Status":
                    predbiljezbas = predbiljezbas.OrderBy(s => s.Status);
                    break;
                case "Status_desc":
                    predbiljezbas = predbiljezbas.OrderByDescending(s => s.Status);
                    break;
                case "Ime":
                    predbiljezbas = predbiljezbas.OrderBy(s => s.Ime);
                    break;
                case "Ime_desc":
                    predbiljezbas = predbiljezbas.OrderByDescending(s => s.Ime);
                    break;
                case "Prezime":
                    predbiljezbas = predbiljezbas.OrderBy(s => s.Prezime);
                    break;
                case "Prezime_desc":
                    predbiljezbas = predbiljezbas.OrderByDescending(s => s.Prezime);
                    break;
                case "Adresa":
                    predbiljezbas = predbiljezbas.OrderBy(s => s.Adresa);
                    break;
                case "Adresa_desc":
                    predbiljezbas = predbiljezbas.OrderByDescending(s => s.Adresa);
                    break;
                default:
                    predbiljezbas = predbiljezbas.OrderBy(s => s.IdSeminar);
                    break;
            }

            if (!String.IsNullOrEmpty(filterKat))
            {
                if (filterKat == "Ime")
                {
                    predbiljezbas = predbiljezbas.Where(s => s.Ime.Contains(searchString));
                }
                else if (filterKat == "Prezime")
                {
                    predbiljezbas = predbiljezbas.Where(s => s.Prezime.Contains(searchString));
                }
                else if (filterKat == "Adresa")
                {
                    predbiljezbas = predbiljezbas.Where(s => s.Adresa.Contains(searchString));
                }
                else if (filterKat == "Email")
                {
                    predbiljezbas = predbiljezbas.Where(s => s.Email.Contains(searchString));
                }
                else if (filterKat == "Telefon")
                {
                    predbiljezbas = predbiljezbas.Where(s => s.Telefon.Contains(searchString));
                }
                else if (filterKat == "Seminar")
                {
                    predbiljezbas = predbiljezbas.Where(s => s.Seminar.Naziv.Contains(searchString));
                }

            }

            if (filter == "true")
            {
                predbiljezbas = predbiljezbas.Where(s => s.Status.Value == true);
            }
            else if (filter == "false")
            {
                predbiljezbas = predbiljezbas.Where(s => s.Status.Value == false);
            }
            else if (filter == "null")
            {
                predbiljezbas = predbiljezbas.Where(s => s.Status.Value == null);
            }
            else if (filter == "all")
            {
                return View(predbiljezbas.ToList());
            }


            return View(predbiljezbas.ToList());
        }
        #endregion

        #region Details
        // GET: Predbiljezbas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Predbiljezba predbiljezba = db.Predbiljezbas.Find(id);
            if (predbiljezba == null)
            {
                return HttpNotFound();
            }
            return View(predbiljezba);
        }
        #endregion

        #region Create
        // GET: Predbiljezbas/Create
        public ActionResult Create(int id)
        {
            ViewBag.IdSeminar = new SelectList(db.Seminars, "IdSeminar", "Naziv");
            Seminar seminar = db.Seminars.Find(id);
            return View();
        }

        // POST: Predbiljezbas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPredbiljezba,Datum,Ime,Prezime,Adresa,Email,Telefon,IdSeminar,Status")] Predbiljezba predbiljezba)
        {
            if (ModelState.IsValid)
            {
                db.Predbiljezbas.Add(predbiljezba);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdSeminar = new SelectList(db.Seminars, "Seminar", "Naziv", predbiljezba.IdSeminar);
            return View(predbiljezba);
        }
        #endregion

        #region Edit
        // GET: Predbiljezbas/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Predbiljezba predbiljezba = db.Predbiljezbas.Find(id);
            if (predbiljezba == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSeminar = new SelectList(db.Seminars, "IdSeminar", "Naziv", predbiljezba.IdSeminar);
            return View(predbiljezba);
        }

        // POST: Predbiljezbas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPredbiljezba,Datum,Ime,Prezime,Adresa,Email,Telefon,IdSeminar,Status,BrojPolaznika")] Predbiljezba predbiljezba)
        {

            if (ModelState.IsValid)
            {


                db.Entry(predbiljezba).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdSeminar = new SelectList(db.Seminars, "IdSeminar", "Naziv", predbiljezba.IdSeminar);
            return View(predbiljezba);
        }
        #endregion

        #region Delete
        // GET: Predbiljezbas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Predbiljezba predbiljezba = db.Predbiljezbas.Find(id);
            if (predbiljezba == null)
            {
                return HttpNotFound();
            }
            return View(predbiljezba);
        }

        // POST: Predbiljezbas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Predbiljezba predbiljezba = db.Predbiljezbas.Find(id);
            db.Predbiljezbas.Remove(predbiljezba);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

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

