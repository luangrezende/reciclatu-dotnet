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
            try
            {
                int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
                Usuarios user = db.Usuarios.Find(idUsuario);
                Agendamentos agendamento = db.Agendamentos.Find(id);
                Endereco endFinal = new Endereco();

                endFinal.tipoRota = 2;
                endFinal.EnderecosOrigem = user.Enderecos.Where(end => end.idStatus == 1).FirstOrDefault();
                endFinal.EnderecosDestino = agendamento.Enderecos;

                return View("_CalcularRota", endFinal);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult MostraEndereco(int id)
        {
            try
            {
                Enderecos endereco = db.Enderecos.Find(id);
                Endereco endFinal = new Endereco();

                endFinal.tipoRota = 1;
                endFinal.descricao = endereco.rua + ", " + endereco.numero + "" + endereco.cidade;

                return View("_MostraEndereco", endFinal);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}