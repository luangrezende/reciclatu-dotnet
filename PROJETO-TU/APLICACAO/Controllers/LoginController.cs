using APLICACAO.Models;
using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    public class LoginController : ConfigController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                if (!VerificaAutenticidade())
                    return View();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CadastrarUsuario()
        {
            return View();
        }

        //METHODS ..............................................
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login usuario)
        {
            try
            {
                if (!VeriricaUsuario(usuario))
                    return Json(new { msg = "Usuário ou senha incorreto(s)", erro = true }, JsonRequestBehavior.AllowGet);

                //GRAVA SESSAO
                Usuarios user = db.Usuarios.Where(u => u.userName == usuario.UserName && u.password == usuario.Password).FirstOrDefault();
                SessionCookies(user);

                return Json(new { erro = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                RemoveCookies();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}