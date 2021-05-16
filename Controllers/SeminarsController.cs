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
    public class SeminarsController : Controller
    {
        private PredbiljezbedbEntities db = new PredbiljezbedbEntities();

        #region index
        // GET: Seminars
        public ActionResult Index(string sortOrder, string searchString, string SearchBy)
        {

            var seminar = from m in db.Seminars select m;

            var predbiljezba = from s in db.Predbiljezbas.Where(s => s.Status == true) select s;

            foreach (var item in seminar)
            {

                item.BrojPolaznika = predbiljezba.Where(s => s.IdSeminar == item.IdSeminar).Count();
            }

            ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";
            ViewBag.DatumSortParm = sortOrder == "Datum" ? "Datum_desc" : "Datum";
            ViewBag.PopunjenSortParm = sortOrder == "Popunjen" ? "Popunjen_desc" : "Popunjen";
            ViewBag.SakriSortParam = sortOrder == "true" ? "false" : "true";
            ViewBag.BrojPolaznikaSortParam = sortOrder == "Broj" ? "Broj_desc" : "Broj";

            switch (sortOrder)
            {
                case "naziv_desc":
                    seminar = seminar.OrderByDescending(s => s.Naziv);
                    break;
                case "Datum":
                    seminar = seminar.OrderBy(s => s.Datum);
                    break;
                case "Datum_desc":
                    seminar = seminar.OrderByDescending(s => s.Datum);
                    break;
                case "Popunjen":
                    seminar = seminar.OrderBy(s => s.Popunjen);
                    break;
                case "Popunjen_desc":
                    seminar = seminar.OrderByDescending(s => s.Popunjen);
                    break;
                case "true":
                    seminar = seminar.Where(s => s.Popunjen == false);
                    break;
                case "false":
                    seminar = seminar.Where(s => s.Popunjen == true);
                    break;
                case "Broj":
                    seminar = seminar.OrderBy(s => s.BrojPolaznika);
                    break;
                case "Broj_desc":
                    seminar = seminar.OrderByDescending(s => s.BrojPolaznika);
                    break;
                default:
                    seminar = seminar.OrderBy(s => s.Naziv);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                if (SearchBy == "Opis")
                {
                    seminar = seminar.Where(s => s.Opis.Contains(searchString));
                }
                else if (SearchBy == "Naziv")
                {
                    seminar = seminar.Where(s => s.Naziv.Contains(searchString));
                }

            }





            return View(seminar);
        }
        #endregion

        #region details
        // GET: Seminars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seminar seminar = db.Seminars.Find(id);
            if (seminar == null)
            {
                return HttpNotFound();
            }
            return View(seminar);
        }
        #endregion

        #region create
        // GET: Seminars/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Seminars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSeminar,Naziv,Opis,Datum,Popunjen")] Seminar seminar)
        {
            if (ModelState.IsValid)
            {
                db.Seminars.Add(seminar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(seminar);
        }
        #endregion

        #region edit
        // GET: Seminars/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seminar seminar = db.Seminars.Find(id);
            if (seminar == null)
            {
                return HttpNotFound();
            }
            return View(seminar);
        }

        // POST: Seminars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSeminar,Naziv,Opis,Datum,Popunjen")] Seminar seminar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seminar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seminar);
        }
        #endregion

        #region delete
        // GET: Seminars/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seminar seminar = db.Seminars.Find(id);
            if (seminar == null)
            {
                return HttpNotFound();
            }
            return View(seminar);
        }

        // POST: Seminars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seminar seminar = db.Seminars.Find(id);
            db.Seminars.Remove(seminar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region prijavi
        public ActionResult Prijavi(int id)
        {
            Seminar seminar = db.Seminars.Find(id);
            ViewBag.IdSeminar = new SelectList(db.Seminars, "IdSeminar", "Naziv");


            return View();
        }

        // POST: Predbiljezbas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Prijavi([Bind(Include = "IdPredbiljezba,Datum,Ime,Prezime,Adresa,Email,Telefon,IdSeminar")] Predbiljezba predbiljezba)
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
