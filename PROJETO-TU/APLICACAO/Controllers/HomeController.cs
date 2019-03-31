using APLICACAO.Models;
using DATABASE;
using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    public class HomeController : Controller
    {
        //CONTROL VARS
        private DbContextTU db;
        private int Cliente = 1;
        private int Empresa = 2;
        private int ADM = 3;
        private int usuarioSessao = 0;

        //DATABASE CONNECTION
        public HomeController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                usuarioSessao = Convert.ToInt16(Request.Cookies["idUsuario"].Value.ToString());
                if (usuarioSessao == 0)
                    return View();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                usuarioSessao = Convert.ToInt16(Request.Cookies["idUsuario"].Value.ToString());

                Usuarios user = db.Usuarios.Find(usuarioSessao);
                List<Agendamentos> agendamentos = new List<Agendamentos>();

                if (user is null)
                    return RedirectToAction("Login");

                //VERIFICA USUARIO
                if (user.idTipoUsuario == Cliente)
                    agendamentos = db.Agendamentos.Where(a => a.idUsuarioSolicita == user.ID).OrderByDescending(a => a.ID).Take(3).ToList();
                else if (user.idTipoUsuario == Empresa)
                    agendamentos = db.Agendamentos.Where(a => a.idUsuarioColeta == user.ID).OrderByDescending(a => a.ID).Take(3).ToList();
                else if (user.idTipoUsuario == ADM)
                    agendamentos = db.Agendamentos.Where(a => a.idUsuarioColeta == user.ID).OrderByDescending(a => a.ID).Take(3).ToList();

                return View(agendamentos);
            }
            catch (Exception)
            {
                return RedirectToAction("Login");
            }
        }

        //METHODS=============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login usuario)
        {
            try
            {
                Usuarios user = db.Usuarios.Where(u => u.userName == usuario.userName && u.password == usuario.password).FirstOrDefault();

                if (user is null)
                    return Json(new { msg = "Usuário ou senha incorreto(s)", erro = true }, JsonRequestBehavior.AllowGet);
                
                //GRAVA SESSAO
                GravaCookie("userName", user.userName.ToString());
                GravaCookie("Nome", user.nome.ToString());
                GravaCookie("idUsuario", user.ID.ToString());
                GravaCookie("tipoUsuario", user.idTipoUsuario.ToString());
                GravaCookie("APIKeyMaps", "AIzaSyCs4V6D66_ZjS8IuH9Lq-xqvUhJIoKLUqA");

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
                string[] allDomainCookes = HttpContext.Request.Cookies.AllKeys;

                foreach (string domainCookie in allDomainCookes)
                {
                    if (domainCookie.Contains("ASPXAUTH"))
                    {
                        var expiredCookie = new HttpCookie(domainCookie)
                        {
                            Expires = DateTime.Now.AddDays(-1),
                            Domain = ".mydomain"
                        };
                        HttpContext.Response.Cookies.Add(expiredCookie);
                    }
                }
                HttpContext.Request.Cookies.Clear();

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        private void GravaCookie(string nomeCookie, string valor)
        {
            HttpCookie cookie = new HttpCookie(nomeCookie);
            cookie.Value = valor;
            TimeSpan tempo = new TimeSpan(0, 10, 0, 0);
            cookie.Expires = DateTime.Now + tempo;
            Response.Cookies.Add(cookie);
        }
    }
}