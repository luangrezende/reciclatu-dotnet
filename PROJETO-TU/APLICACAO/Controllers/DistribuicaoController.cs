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
    public class DistribuicaoController : Controller
    {
        private DbContextTU db;

        public DistribuicaoController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Index()
        {
            int statusAberto = 1;
            List<Agendamentos> agend = db.Agendamentos.Where(a => a.idStatus == statusAberto).ToList();
            return View(agend);
        }

        //METHODS ============================================
        [HttpPost]
        public JsonResult AceitarPedido(int id)
        {
            try
            {
                int statusDistribuido = 2;
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
                return Json("Erro na atualização do registro: " + ex.Message);
            }
        }
    }
}