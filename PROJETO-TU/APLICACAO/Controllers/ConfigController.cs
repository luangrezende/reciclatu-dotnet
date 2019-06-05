using APLICACAO.Models;
using DATABASE;
using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace APLICACAO.Controllers
{
    public class ConfigController : Controller
    {
        protected readonly DbContextTU db;
        protected readonly int Cliente = 1;

        public ConfigController()
        {
            db = new DbContextTU();
            VerificaNotificacao();
        }

        protected void VerificaNotificacao()
        {
            ViewBag.QuantidadeNoficiacao = db.Agendamentos.Select(a => a.vizualizado == 1 && a.UsuariosColeta.ID == 3).Count();
        }

        private void WriteCookie(string nomeCookie, string valor)
        {
            HttpCookie cookie = new HttpCookie(nomeCookie)
            {
                Value = valor
            };
            TimeSpan tempo = new TimeSpan(0, 10, 0, 0);
            cookie.Expires = DateTime.Now + tempo;
            Response.Cookies.Add(cookie);
        }

        protected void RemoveCookies()
        {
            string[] allDomainCookes = HttpContext.Request.Cookies.AllKeys;
            foreach (string domainCookie in allDomainCookes)
            {
                var expiredCookie = new HttpCookie(domainCookie)
                {
                    Expires = DateTime.Now.AddDays(-10),
                };
                HttpContext.Response.Cookies.Add(expiredCookie);
            }
            HttpContext.Request.Cookies.Clear();
        }

        protected bool VerificaAutenticidade()
        {
            string[] allDomainCookes = HttpContext.Request.Cookies.AllKeys;
            foreach (string domainCookie in allDomainCookes)
            {
                if (domainCookie.Contains(".ASPXAUTH"))
                {
                    return true;
                }
            }
            return false;
        }

        protected int PegaUsuarioSessaoAtual()
        {
            return Convert.ToInt32(Request.Cookies["idUsuario"].Value.ToString());
        }

        protected bool VeriricaUsuario(Login usuario)
        {
            return db.Usuarios.Any(u => u.userName == usuario.UserName && u.password == usuario.Password);
        }

        protected void SessionCookies(Usuarios user)
        {
            FormsAuthentication.SetAuthCookie(user.userName, false);
            WriteCookie("userName", user.userName.ToString());
            WriteCookie("Nome", user.nome.ToString());
            WriteCookie("idUsuario", user.ID.ToString());
            WriteCookie("tipoUsuario", user.idTipoUsuario.ToString());
            WriteCookie("APIKeyMaps", "API_MAPS_HERE");
        }
    }
}