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
        [Filters.AutorizaAdmin]
        public ActionResult Index()
        {
            var filaData = db.FilaData.Include(f => f.Cliente).Include(f => f.Setor);
            return View(filaData.ToList());
        }
        public ActionResult FilaCliente(int idSetor, int idCliente)
        {
            //Posição na fila
            List<FilaData> filaHoje = db.FilaData.Where(f => f.IDSetor == idSetor && f.Data == DateTime.Today).Cast<FilaData>().ToList();
            int index = 0, nFila = 0, nAtendidos = 0;

            foreach(FilaData c in filaHoje)
            {
                index++;
                if (c.IDCliente == idCliente)
                {
                    nFila = index;
                }
            }
            index = 0;
            foreach (FilaData c in filaHoje)
            {
                index++;
  
                if (c.Atendido == false && index < nFila)
                {
                    nAtendidos++;
                }
            }

            ViewBag.nFila = nFila;
            ViewBag.nAtendidos = nAtendidos;
            return View();
        }
        public ActionResult FilaAtendente(int idSetor)
        {
            //Conta as pessoas na fila no setor e no dia
            int qtdFila = db.FilaData.Where(f => f.IDSetor == idSetor && f.Data== DateTime.Today).Count();
            int nAtendidos = db.FilaData.Where(f => f.IDSetor == idSetor && f.Data == DateTime.Today).Count() - 
                db.FilaData.Where(f => f.IDSetor == idSetor && f.Data == DateTime.Today && f.Atendido == false).Count() +1;
            ViewBag.qtdFila = qtdFila.ToString();
            ViewBag.nAtendidos = nAtendidos.ToString();
            ViewBag.idSetor = idSetor.ToString();
            return View();
        }
        public ActionResult ProximoFila(int idSetor)
        {
            //Conta as pessoas na fila no setor e no dia
            var clienteAtendido = db.FilaData.Where(f => f.IDSetor == idSetor && f.Data == DateTime.Today && f.Atendido==false).OrderBy(f=> f.ID).FirstOrDefault();
            clienteAtendido.Atendido = true;
            db.Entry(clienteAtendido).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("FilaAtendente", new { idSetor = idSetor });
        }
        //Inserir o Cliente na fila
        public ActionResult EntraNaFila(int idSetor, int idUsuario)
        {
            int idEstabelecimento = (db.Setor.Where(e => e.ID == idSetor).FirstOrDefault()).IDEstabelecimento;
            Cliente idCliente = db.Cliente.Where(c => c.IDUsuario == idUsuario && c.IDEstabelecimento == idEstabelecimento).FirstOrDefault();
            if (idCliente==null)
            {
                return RedirectToAction("Criar","Clientes", new { idEstabelecimento = idEstabelecimento, idUsuario = idUsuario});
            }
            FilaData filaCli = db.FilaData.Where(f => f.IDCliente == idCliente.ID && f.IDSetor == idSetor && f.Atendido == false && f.Data == DateTime.Today).FirstOrDefault();
            if (filaCli!=null)
            {
                return RedirectToAction("FilaCliente", new { idSetor = idSetor, idCliente = idCliente.ID });
            }
            FilaData filaCliente = new FilaData();
            filaCliente.IDSetor = idSetor;
            filaCliente.IDCliente = idCliente.ID;
            filaCliente.Data = DateTime.Today;
            filaCliente.Atendido = false;

            if (ModelState.IsValid)
            {
                db.FilaData.Add(filaCliente);
                db.SaveChanges();
                return RedirectToAction("FilaCliente" , new { idSetor = idSetor, idCliente = idCliente.ID });
            }

            return RedirectToAction("Index");
        }
        [Filters.AutorizaAdmin]
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
        [Filters.AutorizaAdmin]
        // GET: FilaDatas/Create
        public ActionResult Create()
        {
            ViewBag.IDCliente = new SelectList(db.Cliente, "ID", "Registro");
            ViewBag.IDSetor = new SelectList(db.Setor, "ID", "Nome");
            return View();
        }
        [Filters.AutorizaAdmin]
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
        [Filters.AutorizaAdmin]
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
        [Filters.AutorizaAdmin]
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
        [Filters.AutorizaAdmin]
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
        [Filters.AutorizaAdmin]
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
