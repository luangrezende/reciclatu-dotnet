using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APLICACAO.Models
{
    public class UsuarioModel
    {
        [Required]
        [Display(Name = "Senha")]
        public string password { get; set; }

        [Required]
        [Display(Name = "Nova senha")]
        public string newPassword { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string nome { get; set; }

        [Required]
        [Display(Name = "CPF")]
        public string CPF { get; set; }
    }
}