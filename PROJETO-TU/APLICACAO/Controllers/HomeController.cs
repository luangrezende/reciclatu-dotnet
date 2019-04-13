using APLICACAO.Models;
using DATABASE;
using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace APLICACAO.Controllers
{
    [Authorize]
    public class HomeController : ConfigController
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                if (!VerificaAutenticidade())
                    return RedirectToAction("Login");

                Usuarios user = db.Usuarios.Find(usuarioSessao);
                List<Agendamentos> agendamentos = db.Agendamentos.Where(a => (user.idTipoUsuario == Cliente ? a.idUsuarioSolicita == user.ID : a.idUsuarioColeta == user.ID))
                    .OrderByDescending(a => a.ID).Take(3).ToList();

                return View(agendamentos);
            }
            catch (Exception)
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            try
            {
                if (!VerificaAutenticidade())
                    return View();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
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
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}