using APLICACAO.Models;
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
    [Authorize]
    public class UsuarioController : Controller
    {
        //CONTROL VARS
        private readonly DbContextTU db;
        private readonly int Cancelado = 3;
        private readonly int Ativo = 1;
        private readonly int Inativo = 0;
        private readonly int limiteEnderecos = 3;

        //DATABASE CONNECTION
        public UsuarioController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Index()
        {
            int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
            return View(db.Usuarios.Where(e => e.ID == idUsuario).FirstOrDefault());
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
            if (user.Enderecos.Where(c => c.idStatus != Cancelado).Count() >= limiteEnderecos)
                return Json(new { msg = "Você possui muitos endereços cadastrados", erro = true }, JsonRequestBehavior.AllowGet);

            return View("_CadastrarEndereco");
        }

        [HttpGet]
        public ActionResult EditarEndereco(int id)
        {
            return View("_EditarEndereco", db.Enderecos.Where(e => e.ID == id).FirstOrDefault());
        }

        [HttpGet]
        public ActionResult EditarCadastro()
        {
            int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
            return View("_EditarCadastro", db.Usuarios.Where(e => e.ID == idUsuario).FirstOrDefault());
        }

        [HttpGet]
        public ActionResult AlterarSenha()
        {
            return View("_AlterarSenha");
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
                    if (user.Enderecos.Where(e => e.idStatus != Cancelado).Count() == 0)
                        enderecos.idStatus = Ativo;

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

        //[HttpPost]
        //public ActionResult AlterarSenha(Login user)
        //{
        //    return Json("ok");
        //}

        [HttpPost]
        public JsonResult RemoverEndereco(int id)
        {
            try
            {
                Enderecos endereco = db.Enderecos.Find(id);
                if (endereco.Agendamentos.Count() != 0)
                {
                    endereco.idStatus = Cancelado;
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
                Enderecos enderecoAtual = db.Enderecos.Where(e => e.idStatus == Ativo && e.idUsuario == user.ID).FirstOrDefault();
                if (enderecoAtual != null)
                {
                    enderecoAtual.idStatus = Inativo;
                    db.Entry(enderecoAtual).State = EntityState.Modified;
                }

                //ATIVA O SELECIONADO
                Enderecos atualizacao = db.Enderecos.Where(e => e.ID == id && e.idUsuario == user.ID).FirstOrDefault();
                atualizacao.idStatus = Ativo;
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