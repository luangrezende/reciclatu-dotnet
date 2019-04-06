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
    public class LoginController : Controller
    {
        //CONTROL VARS
        private readonly DbContextTU db;

        //DATABASE CONNECTION
        public LoginController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                if (VerificaAutenticidade())
                    return View("Login");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return View();
            }
        }

        //METHODS=============================================
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login usuario)
        {
            try
            {
                bool verificaUsuario = db.Usuarios.Any(u => u.userName == usuario.UserName && u.password == usuario.Password);

                if (!verificaUsuario)
                    return Json(new { msg = "Usuário ou senha incorreto(s)", erro = true }, JsonRequestBehavior.AllowGet);

                //GRAVA SESSAO
                Usuarios user = db.Usuarios.Where(u => u.userName == usuario.UserName && u.password == usuario.Password).FirstOrDefault();
                FormsAuthentication.SetAuthCookie(user.userName, true);
                GravaCookies("userName", user.userName.ToString());
                GravaCookies("Nome", user.nome.ToString());
                GravaCookies("idUsuario", user.ID.ToString());
                GravaCookies("tipoUsuario", user.idTipoUsuario.ToString());
                GravaCookies("APIKeyMaps", "AIzaSyCs4V6D66_ZjS8IuH9Lq-xqvUhJIoKLUqA");

                return Json(new { erro = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult LogOut()
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

        //FUNCTIONS=============================================
        private void GravaCookies(string nomeCookie, string valor)
        {
            HttpCookie cookie = new HttpCookie(nomeCookie)
            {
                Value = valor
            };
            TimeSpan tempo = new TimeSpan(0, 10, 0, 0);
            cookie.Expires = DateTime.Now + tempo;
            Response.Cookies.Add(cookie);
        }

        private void RemoveCookies()
        {
            string[] allDomainCookes = HttpContext.Request.Cookies.AllKeys;
            foreach (string domainCookie in allDomainCookes)
            {
                var expiredCookie = new HttpCookie(domainCookie)
                {
                    Expires = DateTime.Now.AddDays(-10),
                };
                HttpContext.Response.Cookies.Add(expiredCookie);
            }
            HttpContext.Request.Cookies.Clear();
        }

        public bool VerificaAutenticidade()
        {
            string[] allDomainCookes = HttpContext.Request.Cookies.AllKeys;
            foreach (string domainCookie in allDomainCookes)
            {
                if (domainCookie.Contains(".ASPXAUTH"))
                    return false;
            }
            return true;
        }
    }
}