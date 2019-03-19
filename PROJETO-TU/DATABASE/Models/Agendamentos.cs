﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATABASE.Models
{
    public class Agendamentos
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Descricao")]
        public string descricao { get; set; }

        [Required]
        [Display(Name = "Data")]
        public DateTime dtAgendamento { get; set; }

        [Required]
        [Display(Name = "Data atual")]
        public DateTime dtAtual { get; set; }

        [Required]
        [Display(Name = "Hora")]
        public string hora { get; set; }

        [Required]
        public int idUsuarioSolicita { get; set; }

        [Required]
        public int idUsuarioColeta { get; set; }

        [Required]
        public int idStatus { get; set; }

        public int idEndereco { get; set; }

        public int idTipoMaterial { get; set; }

        public virtual Enderecos Enderecos { get; set; }
        public virtual Usuarios UsuariosSolicita { get; set; }
        public virtual Usuarios UsuariosColeta { get; set; }
        public virtual StatusAgendamento StatusAgendamento { get; set; }
        public virtual TipoMaterial TipoMaterial { get; set; }
    }
}
