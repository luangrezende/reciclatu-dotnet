using APLICACAO.Models;
using DATABASE.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    [Authorize]
    public class UsuarioController : ConfigController
    {
        //GLOBAL VARS
        private readonly int Cancelado = 3;
        private readonly int Ativo = 1;
        private readonly int Inativo = 0;
        private readonly int limiteEnderecos = 3;

        //VIEWS ..............................................
        [HttpGet]
        public ActionResult Index()
        {
            int UsuarioSessao = PegaUsuarioSessaoAtual();
            return View(db.Usuarios.Where(e => e.ID == UsuarioSessao).FirstOrDefault());
        }

        [HttpGet]
        public ActionResult CadastrarEndereco()
        {
            int UsuarioSessao = PegaUsuarioSessaoAtual();
            Usuarios user = db.Usuarios.Where(e => e.ID == UsuarioSessao).FirstOrDefault();

            //VERIFICA QUANTIDADE DE ENDEREÇOS
            if (user.Enderecos.Where(c => c.IdStatus != Cancelado).Count() >= limiteEnderecos)
                return Json(new { msg = "Você possui muitos endereços cadastrados", erro = true }, JsonRequestBehavior.AllowGet);

            return View("_CadastrarEndereco");
        }

        [HttpGet]
        public ActionResult EditarEndereco(int id)
        {
            return View("_EditarEndereco", db.Enderecos.Where(e => e.ID == id).FirstOrDefault());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CadastrarUsuario()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditarCadastro()
        {
            int UsuarioSessao = PegaUsuarioSessaoAtual();
            UsuarioModel usuarioModelo = new UsuarioModel();
            Usuarios userAtual = db.Usuarios.Where(e => e.ID == UsuarioSessao).FirstOrDefault();

            usuarioModelo.CPF = userAtual.CPF;
            usuarioModelo.nome = userAtual.nome;

            return View("_EditarCadastro", usuarioModelo);
        }

        [HttpGet]
        public ActionResult EditarSenha()
        {
            return View("_EditarSenha");
        }

        //METHODS ..............................................
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CadastrarUsuario(Usuarios Usuario)
        {
            try
            {
                Usuario.idTipoUsuario = Cliente;
                if (ModelState.IsValid)
                {
                    if (VerificarUsuarioExiste(Usuario))
                    {
                        return Json(new { msg = "Usuário já foi cadastrado", erro = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        db.Usuarios.Add(Usuario);
                        db.SaveChanges();
                    }
                }
                return Json(new { msg = "Usuário cadastrado", erro = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCadastro(UsuarioModel Usuario)
        {
            try
            {
                Usuarios user = db.Usuarios.Find(PegaUsuarioSessaoAtual());
                if (ModelState.IsValid)
                {
                    if (Usuario.password != user.password)
                    {
                        return Json(new { msg = "Senha inválida", erro = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        user.password = Usuario.password;
                        user.nome = Usuario.nome;
                        user.CPF = Usuario.CPF;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return Json(new { msg = "Dados atualizados com sucesso", erro = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CadastrarEndereco(Enderecos enderecos)
        {
            try
            {
                int UsuarioSessao = PegaUsuarioSessaoAtual();
                Usuarios user = db.Usuarios.Find(UsuarioSessao);

                if (ModelState.IsValid)
                {
                    if (user.Enderecos.Where(e => e.IdStatus != Cancelado).Count() == 0)
                        enderecos.IdStatus = Ativo;

                    enderecos.IdUsuario = user.ID;
                    db.Enderecos.Add(enderecos);
                    db.SaveChanges();
                    return Json("Cadastrado com sucesso");
                }
                return Json(new { msg = "Verifique os campos digitados", erro = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditarEndereco(Enderecos endereco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(endereco).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json("Atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RemoverEndereco(int id)
        {
            try
            {
                Enderecos endereco = db.Enderecos.Find(id);
                if (endereco.Agendamentos.Count() != 0)
                {
                    endereco.IdStatus = Cancelado;
                    db.Entry(endereco).State = EntityState.Modified;
                }
                else
                {
                    db.Entry(endereco).State = EntityState.Deleted;
                }
                db.SaveChanges();

                return Json("Removido com sucesso");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AtivarEndereco(int id)
        {
            try
            {
                int UsuarioSessao = PegaUsuarioSessaoAtual();

                //ENCONTRA O ATIVO ATUAL
                Enderecos enderecoAtual = db.Enderecos.Where(e => e.IdStatus == Ativo && e.IdUsuario == UsuarioSessao).FirstOrDefault();
                if (enderecoAtual != null)
                {
                    enderecoAtual.IdStatus = Inativo;
                    db.Entry(enderecoAtual).State = EntityState.Modified;
                }

                //ATIVA O SELECIONADO
                Enderecos atualizacao = db.Enderecos.Where(e => e.ID == id && e.IdUsuario == UsuarioSessao).FirstOrDefault();
                atualizacao.IdStatus = Ativo;

                db.Entry(atualizacao).State = EntityState.Modified;
                db.SaveChanges();

                return Json("Endereço ativado");
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public bool VerificarUsuarioExiste(Usuarios Usuario)
        {
            return db.Usuarios.Any(u => u.userName == Usuario.userName);
        }
    }
}