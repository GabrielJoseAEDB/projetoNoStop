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
    public class SetoresController : Controller
    {
        private noStopEntities db = new noStopEntities();

        // GET: Setores
        public ActionResult Index(int idEstab)
        {
            var setor = db.Setor.Include(s => s.Estabelecimento);
            return View(setor.ToList());
        }

        // GET: Setores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setor setor = db.Setor.Find(id);
            if (setor == null)
            {
                return HttpNotFound();
            }
            return View(setor);
        }

        // GET: Setores/Create
        public ActionResult Create()
        {
            ViewBag.IDEstabelecimento = new SelectList(db.Estabelecimento, "ID", "Nome");
            return View();
        }
        
        // POST: Setores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,IDEstabelecimento")] Setor setor)
        {
            if (ModelState.IsValid)
            {
                db.Setor.Add(setor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDEstabelecimento = new SelectList(db.Estabelecimento, "ID", "Nome", setor.IDEstabelecimento);
            return View(setor);
        }

        // GET: Setores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setor setor = db.Setor.Find(id);
            if (setor == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDEstabelecimento = new SelectList(db.Estabelecimento, "ID", "Nome", setor.IDEstabelecimento);
            return View(setor);
        }

        // POST: Setores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,IDEstabelecimento")] Setor setor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDEstabelecimento = new SelectList(db.Estabelecimento, "ID", "Nome", setor.IDEstabelecimento);
            return View(setor);
        }

        // GET: Setores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setor setor = db.Setor.Find(id);
            if (setor == null)
            {
                return HttpNotFound();
            }
            return View(setor);
        }

        // POST: Setores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Setor setor = db.Setor.Find(id);
            db.Setor.Remove(setor);
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
