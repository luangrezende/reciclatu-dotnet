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
                int UsuarioSessao = PegaUsuarioSessaoAtual();

                if (!VerificaAutenticidade())
                    return RedirectToAction("Index", "Login");

                Usuarios user = db.Usuarios.Find(UsuarioSessao);
                List<Agendamentos> agendamentos = db.Agendamentos.Where(a => (user.idTipoUsuario == Cliente ? a.idUsuarioSolicita == user.ID : a.idUsuarioColeta == user.ID))
                    .OrderByDescending(a => a.ID).Take(3).ToList();

                return View(agendamentos);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}