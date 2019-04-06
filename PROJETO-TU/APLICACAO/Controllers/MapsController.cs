using APLICACAO.Models;
using DATABASE;
using DATABASE.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    public class MapsController : Controller
    {
        //CONTROL VARS
        private DbContextTU db;
        private int rotaPontos = 2;
        private int Ativo = 1;
        private int EnderecoFixo = 1;

        //DATABASE CONNECTION
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
                Endereco enderecoFinal = new Endereco();

                enderecoFinal.tipoRota = rotaPontos;
                enderecoFinal.EnderecosOrigem = user.Enderecos.Where(end => end.idStatus == Ativo).FirstOrDefault();
                enderecoFinal.EnderecosDestino = agendamento.Enderecos;

                return View("_CalcularRota", enderecoFinal);
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
                Endereco enderecoFinal = new Endereco();

                enderecoFinal.tipoRota = EnderecoFixo;
                enderecoFinal.descricao = endereco.rua + ", " + endereco.numero + "" + endereco.cidade;

                return View("_MostraEndereco", enderecoFinal);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}