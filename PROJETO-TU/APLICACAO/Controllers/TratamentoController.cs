using DATABASE;
using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    public class TratamentoController : Controller
    {
        private DbContextTU db;

        public TratamentoController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Index()
        {
            int statusDistribuicao = 2;
            int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());

            return View(db.Agendamentos.Where(a => a.idStatus == statusDistribuicao && a.idUsuarioColeta == idUsuario).ToList());
        }
    }
}