using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATABASE.Models
{
    public class TiposUsuario
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Tipo usuário")]
        public string descricao { get; set; }

        public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
}
