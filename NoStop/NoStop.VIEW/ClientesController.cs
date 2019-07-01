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
    public class ClientesController : Controller
    {
        private noStopEntities db = new noStopEntities();

        // GET: Clientes
        public ActionResult Index()
        {
            var cliente = db.Cliente.Include(c => c.Estabelecimento).Include(c => c.Roles).Include(c => c.Usuario);
            return View(cliente.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.IDEstabelecimento = new SelectList(db.Estabelecimento, "ID", "Nome");
            ViewBag.IDRole = new SelectList(db.Roles, "ID", "Nome");
            ViewBag.IDUsuario = new SelectList(db.Usuario, "ID", "Nome");
            return View();
        }

        // POST: Clientes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Registro,IDUsuario,IDRole,IDEstabelecimento")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDEstabelecimento = new SelectList(db.Estabelecimento, "ID", "Nome", cliente.IDEstabelecimento);
            ViewBag.IDRole = new SelectList(db.Roles, "ID", "Nome", cliente.IDRole);
            ViewBag.IDUsuario = new SelectList(db.Usuario, "ID", "Nome", cliente.IDUsuario);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDEstabelecimento = new SelectList(db.Estabelecimento, "ID", "Nome", cliente.IDEstabelecimento);
            ViewBag.IDRole = new SelectList(db.Roles, "ID", "Nome", cliente.IDRole);
            ViewBag.IDUsuario = new SelectList(db.Usuario, "ID", "Nome", cliente.IDUsuario);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Registro,IDUsuario,IDRole,IDEstabelecimento")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDEstabelecimento = new SelectList(db.Estabelecimento, "ID", "Nome", cliente.IDEstabelecimento);
            ViewBag.IDRole = new SelectList(db.Roles, "ID", "Nome", cliente.IDRole);
            ViewBag.IDUsuario = new SelectList(db.Usuario, "ID", "Nome", cliente.IDUsuario);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
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
