using DATABASE;
using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    [Authorize]
    public class DistribuicaoController : Controller
    {
        //CONTROL VARS
        private readonly DbContextTU db;
        private readonly int statusAberto = 1;
        private readonly int statusDistribuido = 2;

        //DATABASE CONNECTION
        public DistribuicaoController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Agendamentos.Where(a => a.idStatus == statusAberto).ToList());
        }

        //METHODS ============================================
        [HttpPost]
        public JsonResult AceitarPedido(int id)
        {
            try
            {
                int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
                if (ModelState.IsValid)
                {
                    Agendamentos agend = db.Agendamentos.Find(id);
                    Usuarios user = db.Usuarios.Find(idUsuario);

                    //verifica se usuario possui endereco
                    if (user.Enderecos.Count == 0)
                    {
                        return Json("Cadastre um endereço");
                    }

                    agend.idStatus = statusDistribuido;
                    agend.dtAgendamento = DateTime.Now;
                    agend.idUsuarioColeta = user.ID;
                    agend.UsuariosColeta = user; 
                    db.Entry(agend).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json("Agendamento aceito com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}