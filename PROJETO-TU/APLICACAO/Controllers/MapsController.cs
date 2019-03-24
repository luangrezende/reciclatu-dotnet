using DATABASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    public class MapsController : Controller
    {
        private DbContextTU db;

        public MapsController()
        {
            db = new DbContextTU();
        }

        //METHODS ============================================
        [HttpPost]
        public ActionResult CalcularRota(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult MostraEndereco(int id)
        {
            return View("_MostraEndereco");
        }
    }
}