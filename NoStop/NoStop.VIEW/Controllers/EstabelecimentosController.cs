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
    public class EstabelecimentosController : Controller
    {
        private noStopEntities db = new noStopEntities();

        // GET: Estabelecimentos
        public ActionResult Index()
        {
            return View(db.Estabelecimento.ToList());
        }

        public ActionResult EncontrarCliente(int idUsuario,int idEstabelecimento)
        {
            
            ViewBag.cliente = db.Cliente.Where(c => c.IDEstabelecimento == idEstabelecimento && c.IDUsuario == idUsuario).FirstOrDefault();
            return View();
        }
        public ActionResult MeusEstabelecimentos(int idUsuario)
        {
            List<Cliente> listIds = db.Cliente.Where(c => c.IDUsuario == idUsuario).ToList();
            //Lembrar de colocar o parâmetro para o id do cliente que está acessando
            List<EstabelecimentoCliente> vwModel = new List<EstabelecimentoCliente>();
            foreach (var cliId in listIds)
            {
                var joinQuery = (from cli in db.Cliente
                                 join es in db.Estabelecimento
                                 on cli.IDEstabelecimento equals es.ID
                                 where cli.ID == cliId.ID
                                 where cli.Roles.Nome == "admin"
                                 select new
                                 {
                                     IdCliente = cli.ID,
                                     idEstabelecimento = es.ID,
                                     NomeEstabelecimento = es.Nome,
                                     Endereco = es.Endereco
                                 }).ToList();

                foreach (var item in joinQuery)
                {
                    vwModel.Add(new EstabelecimentoCliente()
                    {
                        IdCliente = item.IdCliente,
                        idEstabelecimento = item.idEstabelecimento,
                        NomeEstabelecimento = item.NomeEstabelecimento,
                        Endereco = item.Endereco
                    });
                }
            }
            return View(vwModel);
        }
        public ActionResult ExibirClientes(int? idEstab)
        {
            if (idEstab == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ClientePermissoes> vwModel = new List<ClientePermissoes>();
            var joinQuery = (from cli in db.Cliente
                             join es in db.Estabelecimento
                             on cli.IDEstabelecimento equals es.ID
                             where cli.IDEstabelecimento == idEstab
                             select new
                             {
                                 IdCliente = cli.ID,
                                 idEstabelecimento = es.ID,
                                 NomeCliente = cli.Usuario.Nome,
                                 Role = cli.IDRole
                             }).ToList();
            if (joinQuery == null)
            {
                return HttpNotFound();
            }
            foreach (var item in joinQuery)
            {
                vwModel.Add(new ClientePermissoes()
                {
                    IDCliente = item.IdCliente,
                    IDEstabelecimento = item.idEstabelecimento,
                    NomeCliente = item.NomeCliente,
                    Role = item.Role
                });
            }
            return View(vwModel);
        }
        [Filters.AutorizaAdmin]
        // GET: Estabelecimentos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estabelecimento estabelecimento = db.Estabelecimento.Find(id);
            if (estabelecimento == null)
            {
                return HttpNotFound();
            }

            return View(estabelecimento);
        }
        [Filters.AutorizaAdmin]
        // GET: Estabelecimentos/Create
        public ActionResult Create()
        {
            if (Session["usuarioLogadoID"] != null && Session["Role"].ToString() == "admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Erro","Home");
            }
        }

        // POST: Estabelecimentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Filters.AutorizaAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,CNPJ,Endereco")] Estabelecimento estabelecimento)
        {
            if (ModelState.IsValid)
            {
                db.Estabelecimento.Add(estabelecimento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estabelecimento);
        }
        [Filters.AutorizaAdmin]
        // GET: Estabelecimentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estabelecimento estabelecimento = db.Estabelecimento.Find(id);
            if (estabelecimento == null)
            {
                return HttpNotFound();
            }
            return View(estabelecimento);
        }
        [Filters.AutorizaAdmin]
        // POST: Estabelecimentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,CNPJ,Endereco")] Estabelecimento estabelecimento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estabelecimento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estabelecimento);
        }
        [Filters.AutorizaAdmin]
        // GET: Estabelecimentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estabelecimento estabelecimento = db.Estabelecimento.Find(id);
            if (estabelecimento == null)
            {
                return HttpNotFound();
            }
            return View(estabelecimento);
        }
        [Filters.AutorizaAdmin]
        // POST: Estabelecimentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Estabelecimento estabelecimento = db.Estabelecimento.Find(id);
                db.Estabelecimento.Remove(estabelecimento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                
                return RedirectToAction("Index");
            }
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
