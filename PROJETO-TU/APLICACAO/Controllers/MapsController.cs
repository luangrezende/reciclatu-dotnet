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
        private readonly DbContextTU db;
        private readonly int rotaPontos = 2;
        private readonly int Ativo = 1;
        private readonly int EnderecoFixo = 1;

        //DATABASE CONNECTION
        public MapsController()
        {
            db = new DbContextTU();
        }

        //METHODS ..............................................
        [HttpPost]
        public ActionResult CalcularRota(int id)
        {
            try
            {
                int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
                Usuarios user = db.Usuarios.Find(idUsuario);
                Agendamentos agendamento = db.Agendamentos.Find(id);
                Endereco enderecoFinal = new Endereco
                {
                    tipoRota = rotaPontos,
                    EnderecosOrigem = user.Enderecos.Where(end => end.IdStatus == Ativo).FirstOrDefault(),
                    EnderecosDestino = agendamento.Enderecos
                };

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
                Endereco enderecoFinal = new Endereco
                {
                    tipoRota = EnderecoFixo,
                    descricao = endereco.Rua + ", " + endereco.Numero + "" + endereco.Cidade
                };

                return View("_MostraEndereco", enderecoFinal);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}