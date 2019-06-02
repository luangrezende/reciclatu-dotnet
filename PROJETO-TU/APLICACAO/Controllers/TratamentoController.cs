using DATABASE;
using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    [Authorize]
    public class TratamentoController : ConfigController
    {
        private readonly int statusDistribuicao = 2;

        //VIEWS ..............................................
        [HttpGet]
        public ActionResult Index()
        {
            int UsuarioSessao = PegaUsuarioSessaoAtual();

            return View(db.Agendamentos.Where(a => a.idUsuarioColeta == UsuarioSessao).ToList());
        }
    }
}