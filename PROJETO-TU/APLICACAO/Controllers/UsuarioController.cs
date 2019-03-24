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
            ViewBag.idTipoUsuario = new SelectList(db.TiposUsuario, "ID", "descricao");
            return View();
        }

        [HttpGet]
        public ActionResult CadastrarEndereco()
        {
            int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
            Usuarios user = db.Usuarios.Where(e => e.ID == idUsuario).FirstOrDefault();

            //VERIFICA QUANTIDADE DE ENDEREÇOS
            if (user.Enderecos.Count() >= 3)
            {
                var msg = new
                {
                    mensagem = "Você possui muitos endereços cadastrados",
                    erro = 1
                };

                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return View("_CadastrarEndereco");
        }

        //METHODS ============================================
        [HttpGet]
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
                return Json("Erro na inserção do registro: " + ex.Message);
            }
        }

        [HttpPost]
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
                return Json("Erro na edição do registro: " + ex.Message);
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
                    //VERIFICA QUANTIDADE DE ENDEREÇOS
                    if (user.Enderecos.Count() >= 3)
                        return Json("Você possui muitos endereços cadastrados", JsonRequestBehavior.AllowGet);

                    enderecos.idUsuario = user.ID;
                    db.Enderecos.Add(enderecos);
                    db.SaveChanges();
                }
                return Json("Cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                return Json("Erro na inserção do registro: " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult EditarEndereco(Enderecos endereco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(endereco).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json("Erro na edição do registro: " + ex.Message);
            }
        }

        [HttpPost]
        public JsonResult RemoverEndereco(int id)
        {
            try
            {
                Enderecos endereco = db.Enderecos.Find(id);
                db.Entry(endereco).State = EntityState.Deleted;
                db.SaveChanges();

                return Json("Removido com sucesso");
            }
            catch (Exception ex)
            {
                return Json("Erro na edição do registro: " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AtivarEndereco(int id)
        {
            try
            {
                int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
                Usuarios user = db.Usuarios.Where(e => e.ID == idUsuario).FirstOrDefault();

                Enderecos enderecoAtual = db.Enderecos.Where(e => e.idStatus == 1 && e.idUsuario == user.ID).FirstOrDefault();
                enderecoAtual.idStatus = 0;
                db.Entry(enderecoAtual).State = EntityState.Modified;

                Enderecos atualizacao = db.Enderecos.Where(e => e.ID == id && e.idUsuario == user.ID).FirstOrDefault();
                atualizacao.idStatus = 1;
                db.Entry(atualizacao).State = EntityState.Modified;
                db.SaveChanges();

                return Json("Endereço ativado");
            }
            catch (Exception ex)
            {
                return Json("Erro na edição do registro: " + ex.Message);
            }
        }
    }
}