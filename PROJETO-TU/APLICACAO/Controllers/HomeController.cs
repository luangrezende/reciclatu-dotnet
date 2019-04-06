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
    public class HomeController : Controller
    {
        //CONTROL VARS
        private readonly DbContextTU db;
        private readonly int Cliente = 1;
        private readonly int Empresa = 2;
        private readonly int ADM = 3;
        private int usuarioSessao = 0;
        private string cookieAuth = "";

        //DATABASE CONNECTION
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
                if (VerificaAutenticidade())
                    return RedirectToAction("Index", "Login");

                Usuarios user = db.Usuarios.Find(usuarioSessao);
                List<Agendamentos> agendamentos = new List<Agendamentos>();

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

        //FUNCTIONS=============================================
        public bool VerificaAutenticidade()
        {
            usuarioSessao = Convert.ToInt16(Request.Cookies["idUsuario"].Value.ToString());
            cookieAuth = Request.Cookies[".ASPXAUTH"].Value.ToString();

            if (usuarioSessao == 0 || cookieAuth == "")
                return true;

            return false;
        }   
    }
}