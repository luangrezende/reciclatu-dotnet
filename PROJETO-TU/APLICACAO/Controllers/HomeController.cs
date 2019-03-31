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

        private int usuarioAtual = 1; //teste apenas

        //DATABASE CONNECTION
        public HomeController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                Usuarios user = db.Usuarios.Find(usuarioAtual);
                List<Agendamentos> agendamentos = new List<Agendamentos>();

                if (user is null)
                {
                    return Json("Cadastre um usuário");
                }

                //GRAVA SESSAO
                GravaCookie("userName", user.userName.ToString());
                GravaCookie("Nome", user.nome.ToString());
                GravaCookie("idUsuario", user.ID.ToString());
                GravaCookie("tipoUsuario", user.idTipoUsuario.ToString());
                GravaCookie("APIKeyMaps", "AIzaSyCs4V6D66_ZjS8IuH9Lq-xqvUhJIoKLUqA");

                //VERIFICA USUARIO
                if (user.idTipoUsuario == Cliente)
                    agendamentos = db.Agendamentos.Where(a => a.idUsuarioSolicita == user.ID).OrderByDescending(a => a.ID).Take(3).ToList();
                else if (user.idTipoUsuario == Empresa)
                    agendamentos = db.Agendamentos.Where(a => a.idUsuarioColeta == user.ID).OrderByDescending(a => a.ID).Take(3).ToList();
                else if (user.idTipoUsuario == ADM)
                    agendamentos = db.Agendamentos.Where(a => a.idUsuarioColeta == user.ID).OrderByDescending(a => a.ID).Take(3).ToList();

                return View(agendamentos);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        //METHODS=============================================
        private void GravaCookie(string nomeCookie, string valor)
        {
            HttpCookie cookie = new HttpCookie(nomeCookie);
            cookie.Value = valor;
            TimeSpan tempo = new TimeSpan(0, 00, 0, 5);
            cookie.Expires = DateTime.Now + tempo;
            Response.Cookies.Add(cookie);
        }
    }
}