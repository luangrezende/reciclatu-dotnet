using DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APLICACAO.Models
{
    public class Endereco
    {
        public string rua { get; set; }

        public string complemento { get; set; }

        public string cidade { get; set; }

        public int numero { get; set; }

        public int CEP { get; set; }

        public string descricao { get; set; }

        public int tipoRota { get; set; }

        public virtual Enderecos EnderecosOrigem { get; set; }
        public virtual Enderecos EnderecosDestino { get; set; }
    }
}