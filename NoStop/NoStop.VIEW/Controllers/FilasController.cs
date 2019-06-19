using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NoStop.MODEL;

namespace NoStop.VIEW
{
    public class FilasController : Controller
    {
        private noStopEntities db = new noStopEntities();

        // GET: Filas
        public ActionResult Index()
        {
            var fila = db.Fila.Include(f => f.Setor);
            return View(fila.ToList());
        }

        // GET: Filas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fila fila = db.Fila.Find(id);
            if (fila == null)
            {
                return HttpNotFound();
            }
            return View(fila);
        }

        // GET: Filas/Create
        public ActionResult Create()
        {
            ViewBag.IDSetor = new SelectList(db.Setor, "ID", "Nome");
            return View();
        }

        // POST: Filas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDSetor")] Fila fila)
        {
            if (ModelState.IsValid)
            {
                db.Fila.Add(fila);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDSetor = new SelectList(db.Setor, "ID", "Nome", fila.IDSetor);
            return View(fila);
        }

        // GET: Filas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fila fila = db.Fila.Find(id);
            if (fila == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDSetor = new SelectList(db.Setor, "ID", "Nome", fila.IDSetor);
            return View(fila);
        }

        // POST: Filas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDSetor")] Fila fila)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fila).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDSetor = new SelectList(db.Setor, "ID", "Nome", fila.IDSetor);
            return View(fila);
        }

        // GET: Filas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fila fila = db.Fila.Find(id);
            if (fila == null)
            {
                return HttpNotFound();
            }
            return View(fila);
        }

        // POST: Filas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fila fila = db.Fila.Find(id);
            db.Fila.Remove(fila);
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
