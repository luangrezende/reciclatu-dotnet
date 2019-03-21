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
        private DbContextTU db;

        public HomeController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                Usuarios user = db.Usuarios.Find(1);
                List<Agendamentos> agendamentos = new List<Agendamentos>();

                if (user is null)
                {
                    return Json("Cadastre um usuário");
                }

                GravaCookie("userName", user.userName.ToString());
                GravaCookie("Nome", user.nome.ToString());
                GravaCookie("idUsuario", user.ID.ToString());
                GravaCookie("tipoUsuario", user.idTipoUsuario.ToString());

                //VERIFICA USUARIO
                if (user.idTipoUsuario == 1)//cliente
                    agendamentos = db.Agendamentos.Where(a => a.idUsuarioSolicita == user.ID).OrderByDescending(a => a.ID).Take(3).ToList();
                else if (user.idTipoUsuario == 2 || user.idTipoUsuario == 3) //empresa ou admin
                    agendamentos = db.Agendamentos.Where(a => a.idUsuarioColeta == user.ID).OrderByDescending(a => a.ID).Take(3).ToList();

                return View(agendamentos);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Informacoes(int id)
        {
            try
            {
                return View("_Informacoes", db.Agendamentos.Where(a => a.ID == id));
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        //METHODS=============================================
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