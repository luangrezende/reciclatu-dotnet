using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATABASE.Models
{
    public class Usuarios
    {
        public int ID { get; set; }

        public int idTipoUsuario { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        public string userName { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string nome { get; set; }

        [Required]
        [Display(Name = "CPF")]
        public int CPF { get; set; }

        public virtual TiposUsuario TiposUsuario { get; set; }
        public virtual ICollection<Agendamentos> Agendamentos { get; set; }
        public virtual ICollection<Enderecos> Enderecos { get; set; }
    }
}
