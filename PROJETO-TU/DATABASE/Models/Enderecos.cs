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
        public string Rua { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Pais")]
        public string Pais { get; set; }

        [Required]
        [Display(Name = "Número")]
        public int Numero { get; set; }

        [Display(Name = "Bairro")]
        public int Bairro { get; set; }

        [Required]
        [Display(Name = "CEP")]
        public string CEP { get; set; }

        [Required]
        [Display(Name = "Descrição do endereço")]
        public string Descricao { get; set; }

        public int IdUsuario { get; set; }

        public int IdStatus { get; set; }

        public virtual Usuarios Usuarios { get; set; }

        public virtual ICollection<Agendamentos> Agendamentos { get; set; }
    }
}
