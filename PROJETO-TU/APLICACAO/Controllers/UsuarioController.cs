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
            return View();
        }

        //METHODS ============================================
        [HttpPost]
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
                if (ModelState.IsValid)
                {
                    Usuarios user = db.Usuarios.Find(idUsuario);
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
    }
}