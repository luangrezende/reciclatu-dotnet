using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APLICACAO.Models
{
    public class Login
    {
        [Required]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }
    }
}