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
                //tem que inicializar as tabelas de tipos(statusAgendamento e tipoUsuario)
                Usuarios user = db.Usuarios.Find(1);
                if (user != null)
                {
                    GravaCookie("userName", user.userName.ToString());
                    GravaCookie("idUsuario", user.ID.ToString());
                    GravaCookie("tipoUsuario", user.idTipoUsuario.ToString());

                }
                return View(db.Agendamentos.Where(a => a.idUsuarioSolicita == user.ID).OrderByDescending(a => a.ID).Take(3).ToList());
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