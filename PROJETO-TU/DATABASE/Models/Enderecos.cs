using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATABASE.Models
{
    public class Enderecos
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Endereco")]
        public string endereco { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string descricao { get; set; }

        public int idUsuario { get; set; }

        public int idStatus { get; set; }

        public virtual Usuarios Usuarios { get; set; }

        public virtual ICollection<Agendamentos> Agendamentos { get; set; }
    }
}
