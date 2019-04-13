using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APLICACAO.Models
{
    public class Endereco
    {
        public string Descricao { get; set; }

        public int TipoRota { get; set; }

        public virtual Enderecos EnderecosOrigem { get; set; }

        public virtual Enderecos EnderecosDestino { get; set; }
    }
}