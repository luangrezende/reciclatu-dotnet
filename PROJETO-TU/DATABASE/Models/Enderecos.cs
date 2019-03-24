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
        [Display(Name = "Rua")]
        public string rua { get; set; }

        [Display(Name = "Complemento")]
        public string complemento { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        public string cidade { get; set; }

        [Required]
        [Display(Name = "Número")]
        public int numero { get; set; }

        [Required]
        [Display(Name = "CEP")]
        public int CEP { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string descricao { get; set; }

        public int idUsuario { get; set; }

        public int idStatus { get; set; }

        public virtual Usuarios Usuarios { get; set; }

        public virtual ICollection<Agendamentos> Agendamentos { get; set; }
    }
}
