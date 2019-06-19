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
    public class FilaClientesController : Controller
    {
        private noStopEntities db = new noStopEntities();

        // GET: FilaClientes
        public ActionResult Index()
        {
            var filaCliente = db.FilaCliente.Include(f => f.Cliente).Include(f => f.Fila);
            return View(filaCliente.ToList());
        }

        // GET: FilaClientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilaCliente filaCliente = db.FilaCliente.Find(id);
            if (filaCliente == null)
            {
                return HttpNotFound();
            }
            return View(filaCliente);
        }

        // GET: FilaClientes/Create
        public ActionResult Create()
        {
            ViewBag.IDCliente = new SelectList(db.Cliente, "ID", "Registro");
            ViewBag.IDFila = new SelectList(db.Fila, "ID", "ID");
            return View();
        }

        // POST: FilaClientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDCliente,IDFila,Data")] FilaCliente filaCliente)
        {
            if (ModelState.IsValid)
            {
                db.FilaCliente.Add(filaCliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCliente = new SelectList(db.Cliente, "ID", "Registro", filaCliente.IDCliente);
            ViewBag.IDFila = new SelectList(db.Fila, "ID", "ID", filaCliente.IDFila);
            return View(filaCliente);
        }

        // GET: FilaClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilaCliente filaCliente = db.FilaCliente.Find(id);
            if (filaCliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCliente = new SelectList(db.Cliente, "ID", "Registro", filaCliente.IDCliente);
            ViewBag.IDFila = new SelectList(db.Fila, "ID", "ID", filaCliente.IDFila);
            return View(filaCliente);
        }

        // POST: FilaClientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDCliente,IDFila,Data")] FilaCliente filaCliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filaCliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCliente = new SelectList(db.Cliente, "ID", "Registro", filaCliente.IDCliente);
            ViewBag.IDFila = new SelectList(db.Fila, "ID", "ID", filaCliente.IDFila);
            return View(filaCliente);
        }

        // GET: FilaClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilaCliente filaCliente = db.FilaCliente.Find(id);
            if (filaCliente == null)
            {
                return HttpNotFound();
            }
            return View(filaCliente);
        }

        // POST: FilaClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FilaCliente filaCliente = db.FilaCliente.Find(id);
            db.FilaCliente.Remove(filaCliente);
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
