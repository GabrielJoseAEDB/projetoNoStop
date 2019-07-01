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

        public ActionResult MeusEstabelecimentos()
        {
            int idCliente = 1;
            //Lembrar de colocar o parâmetro para o id do cliente que está acessando
            List<EstabelecimentoCliente> vwModel = new List<EstabelecimentoCliente>();
            var joinQuery = (from cli in db.Cliente
                             join es in db.Estabelecimento
                             on cli.IDEstabelecimento equals es.ID
                             where cli.ID == idCliente
                            select new
                            {
                            IdCliente= cli.ID,
                            idEstabelecimento = es.ID,
                            NomeEstabelecimento = es.Nome,
                            Endereco = es.Endereco
                            }).ToList();

            foreach(var item in joinQuery)
            {
                vwModel.Add(new EstabelecimentoCliente()
                {
                    IdCliente = item.IdCliente,
                    idEstabelecimento = item.idEstabelecimento,
                    NomeEstabelecimento = item.NomeEstabelecimento,
                    Endereco = item.Endereco
                });
            }
            return View(vwModel);
        }
        public ActionResult ExibirClientes(int? idEstab)
        {
            if (idEstab == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Lembrar de colocar o parâmetro para o id do cliente que está acessando
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
                                 Adm = cli.Role
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
                    Role = item.Adm
                });
            }
            return View(vwModel);
        }
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

        // POST: Estabelecimentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estabelecimento estabelecimento = db.Estabelecimento.Find(id);
            db.Estabelecimento.Remove(estabelecimento);
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
