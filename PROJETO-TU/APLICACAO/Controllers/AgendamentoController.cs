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
    public class AgendamentoController : ConfigController
    {
        //GLOBAL VARS
        private readonly int statusAberto = 1;
        private readonly int cancelado = 4;
        private readonly int finalizado = 3;

        //VIEWS ..............................................
        [HttpGet]
        public ActionResult Index()
        {
            int UsuarioSessao = PegaUsuarioSessaoAtual();
            return View(db.Agendamentos.Where(a => a.idUsuarioSolicita == UsuarioSessao).ToList());
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            try
            {
                int UsuarioSessao = PegaUsuarioSessaoAtual();
                Usuarios user = db.Usuarios.Find(UsuarioSessao);

                //verifica se usuario possui endereco
                if (user.Enderecos.Where(c => c.IdStatus != cancelado).Count() == 0)
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

        //METHODS ..............................................
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Cadastrar(Agendamentos agendamento)
        {
            try
            {
                int UsuarioSessao = PegaUsuarioSessaoAtual();

                if (ModelState.IsValid)
                {
                    agendamento.idUsuarioSolicita = UsuarioSessao;
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
        public JsonResult CheckOut(int id)
        {
            try
            {
                Agendamentos agendamento = db.Agendamentos.Find(id);
                agendamento.idStatus = finalizado;

                ModificarAgendamento(agendamento);
                db.SaveChanges();

                return Json("Check-out com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CancelarPedido(int id)
        {
            try
            {
                Agendamentos agendamento = db.Agendamentos.Find(id);
                agendamento.idStatus = cancelado;
                ModificarAgendamento(agendamento);
                db.SaveChanges();

                return Json("Pedido cancelado com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        private void ModificarAgendamento(Agendamentos agendamento)
        {
            agendamento.vizualizado = 1;
            db.Entry(agendamento).State = EntityState.Modified;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Agendamentos agendamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ModificarAgendamento(agendamento);
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