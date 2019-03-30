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
    public class AgendamentoController : Controller
    {
        private DbContextTU db;

        public AgendamentoController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Index()
        {
            int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
            ViewBag.AguardandoAtendimento = db.Agendamentos.Where(a => a.idUsuarioSolicita == idUsuario && a.UsuariosColeta == null).ToList();

            return View(db.Agendamentos.Where(a => a.idUsuarioSolicita == idUsuario && a.UsuariosColeta != null).ToList());
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            try
            {
                int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
                Usuarios user = db.Usuarios.Find(idUsuario);

                //verifica se usuario possui endereco
                if (user.Enderecos.Count == 0)
                {
                    var mensagem = new
                    {
                        msg = "Cadastre um endereço!",
                    };
                    return Json(mensagem, JsonRequestBehavior.AllowGet);
                }

                ViewBag.Enderecos = db.Enderecos.Where(end => end.idUsuario == user.ID).ToList();
                ViewBag.TipoMaterial = db.TipoMaterial.ToList();
                return View();
            }
            catch (Exception ex)
            {
                var mensagem = new
                {
                    msg = "Erro na pagina: " + ex.Message,
                };
                return Json(mensagem, JsonRequestBehavior.AllowGet);
            }
        }

        //METHODS ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Cadastrar(Agendamentos agendamento)
        {
            try
            {
                int statusAberto = 1;
                int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value);

                if (ModelState.IsValid)
                {
                    agendamento.idUsuarioSolicita = idUsuario;
                    agendamento.dtAbertura = DateTime.Now;
                    agendamento.dtAgendamento = agendamento.dtAbertura; //pensar em uma solucao
                    agendamento.idStatus = statusAberto;

                    db.Agendamentos.Add(agendamento);
                    db.SaveChanges();
                }
                return Json("Cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Agendamentos agendamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(agendamento).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json("Cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InfoAgendamento(int id)
        {
            try
            {
                Agendamentos agend = db.Agendamentos.Find(id);
                return View("_Informacoes", agend);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}