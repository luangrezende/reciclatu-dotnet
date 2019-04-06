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
    public class AgendamentoController : Controller
    {
        //CONTROL VARS
        private readonly DbContextTU db;
        private readonly int statusAberto = 1;
        private readonly int Cancelado = 3;

        //DATABASE CONNECTION
        public AgendamentoController()
        {
            db = new DbContextTU();
        }

        //VIEWS===============================================
        [HttpGet]
        public ActionResult Index()
        {
            int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
            return View(db.Agendamentos.Where(a => a.idUsuarioSolicita == idUsuario).ToList());
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            try
            {
                int idUsuario = Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
                Usuarios user = db.Usuarios.Find(idUsuario);

                //verifica se usuario possui endereco
                if (user.Enderecos.Where(c => c.idStatus != Cancelado).Count() == 0)
                {
                    return RedirectToAction("Index", "Usuario");
                }

                ViewBag.Enderecos = user.Enderecos.ToList();
                ViewBag.TipoMaterial = db.TipoMaterial.ToList();

                return View();
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        //METHODS ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Cadastrar(Agendamentos agendamento)
        {
            try
            {
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