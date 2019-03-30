using DATABASE;
using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    public class UsuarioController : Controller
    {
        private DbContextTU db;

        public UsuarioController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Index()
        {
            int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
            return View(db.Usuarios.Where(e => e.ID == idUsuario).ToList());
        }

        [HttpGet]
        public ActionResult CadastrarUsuario()
        {
            ViewBag.TiposUsuario = db.TiposUsuario.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult CadastrarEndereco()
        {
            int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
            Usuarios user = db.Usuarios.Where(e => e.ID == idUsuario).FirstOrDefault();

            //VERIFICA QUANTIDADE DE ENDEREÇOS
            if (user.Enderecos.Where(c => c.idStatus != 3).Count() >= 3)
            {
                var msg = new
                {
                    mensagem = "Você possui muitos endereços cadastrados",
                    erro = true
                };

                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return View("_CadastrarEndereco");
        }

        [HttpGet]
        public ActionResult EditarEndereco(int id)
        {
            return View("_EditarEndereco", db.Enderecos.Where(e => e.ID == id).FirstOrDefault());
        }

        //METHODS ============================================
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarUsuario(Usuarios Usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuarios.Add(Usuario);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(Usuarios Usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(Usuario).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CadastrarEndereco(Enderecos enderecos)
        {
            try
            {
                int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
                Usuarios user = db.Usuarios.Find(idUsuario);

                if (ModelState.IsValid)
                {
                    if (user.Enderecos.Count() == 0)
                        enderecos.idStatus = 1;

                    enderecos.idUsuario = user.ID;
                    db.Enderecos.Add(enderecos);
                    db.SaveChanges();
                }
                return Json("Cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditarEndereco(Enderecos endereco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(endereco).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json("Atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RemoverEndereco(int id)
        {
            try
            {
                Enderecos endereco = db.Enderecos.Find(id);
                if (endereco.Agendamentos.Count() != 0)
                {
                    endereco.idStatus = 3; //desativado
                    db.Entry(endereco).State = EntityState.Modified;
                }
                else
                {
                    db.Entry(endereco).State = EntityState.Deleted;
                }
                db.SaveChanges();

                return Json("Removido com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AtivarEndereco(int id)
        {
            try
            {
                int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
                Usuarios user = db.Usuarios.Where(e => e.ID == idUsuario).FirstOrDefault();

                //ENCONTRA O ATIVO ATUAL
                Enderecos enderecoAtual = db.Enderecos.Where(e => e.idStatus == 1 && e.idUsuario == user.ID).FirstOrDefault();
                enderecoAtual.idStatus = 0;
                db.Entry(enderecoAtual).State = EntityState.Modified;


                //ATIVA O SELECIONADO
                Enderecos atualizacao = db.Enderecos.Where(e => e.ID == id && e.idUsuario == user.ID).FirstOrDefault();
                atualizacao.idStatus = 1;
                db.Entry(atualizacao).State = EntityState.Modified;
                db.SaveChanges();

                return Json("Endereço ativado");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}