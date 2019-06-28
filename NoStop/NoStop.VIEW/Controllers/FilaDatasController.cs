using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NoStop.MODEL;
using NoStop.MODEL.ViewModels;

namespace NoStop.VIEW
{
    public class FilaDatasController : Controller
    {
        private noStopEntities db = new noStopEntities();

        // GET: FilaDatas
        public ActionResult Index()
        {
            var filaData = db.FilaData.Include(f => f.Cliente).Include(f => f.Setor);
            return View(filaData.ToList());
        }
        public ActionResult FilaCliente(int idSetor)
        {
            //Posição na fila
            int qtdFila = db.FilaData.Where(f => f.IDSetor == idSetor && f.Data == DateTime.Today).Count();
            int nAtendidos = db.FilaData.Where(f => f.IDSetor == idSetor && f.Data == DateTime.Today && f.Atendido==false).Count();
            var vwBag = new qtdFila {
                qtdNaFila = qtdFila,
            nAtendidos = nAtendidos
            };
            ViewBag.vwBag = vwBag;
            return View();
        }
        public ActionResult FilaAtendente(int idSetor)
        {
            //Conta as pessoas na fila no setor e no dia
            int qtdFila = db.FilaData.Where(f => f.IDSetor == idSetor && f.Data== DateTime.Today).Count();
            ViewBag.Message = qtdFila.ToString();
            return View();
        }
        public ActionResult ProximoFila(int idSetor)
        {
            //Conta as pessoas na fila no setor e no dia
            var clienteAtendido = db.FilaData.Where(f => f.IDSetor == idSetor && f.Data == DateTime.Today).FirstOrDefault();
            clienteAtendido.Atendido = true;
            db.Entry(clienteAtendido).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("FilaAtendente");
        }
        //Inserir o Cliente na fila
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EntraNaFila(int idSetor, int idCliente)
        {
            FilaData filaCliente = new FilaData();
            filaCliente.IDSetor = idSetor;
            filaCliente.IDCliente = idCliente;
            filaCliente.Data = DateTime.Today;
            filaCliente.Atendido = false;

            if (ModelState.IsValid)
            {
                db.FilaData.Add(filaCliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        // GET: FilaDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilaData filaData = db.FilaData.Find(id);
            if (filaData == null)
            {
                return HttpNotFound();
            }
            return View(filaData);
        }

        // GET: FilaDatas/Create
        public ActionResult Create()
        {
            ViewBag.IDCliente = new SelectList(db.Cliente, "ID", "Registro");
            ViewBag.IDSetor = new SelectList(db.Setor, "ID", "Nome");
            return View();
        }

        // POST: FilaDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDCliente,IDSetor,Data")] FilaData filaData)
        {
            if (ModelState.IsValid)
            {
                db.FilaData.Add(filaData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCliente = new SelectList(db.Cliente, "ID", "Registro", filaData.IDCliente);
            ViewBag.IDSetor = new SelectList(db.Setor, "ID", "Nome", filaData.IDSetor);
            return View(filaData);
        }
        
        // GET: FilaDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilaData filaData = db.FilaData.Find(id);
            if (filaData == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCliente = new SelectList(db.Cliente, "ID", "Registro", filaData.IDCliente);
            ViewBag.IDSetor = new SelectList(db.Setor, "ID", "Nome", filaData.IDSetor);
            return View(filaData);
        }

        // POST: FilaDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDCliente,IDSetor,Data")] FilaData filaData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filaData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCliente = new SelectList(db.Cliente, "ID", "Registro", filaData.IDCliente);
            ViewBag.IDSetor = new SelectList(db.Setor, "ID", "Nome", filaData.IDSetor);
            return View(filaData);
        }

        // GET: FilaDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilaData filaData = db.FilaData.Find(id);
            if (filaData == null)
            {
                return HttpNotFound();
            }
            return View(filaData);
        }

        // POST: FilaDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FilaData filaData = db.FilaData.Find(id);
            db.FilaData.Remove(filaData);
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
