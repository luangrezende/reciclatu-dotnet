using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATABASE.Models
{
    public class StatusAgendamento
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Descricao")]
        public string descricao { get; set; }

        public virtual ICollection<Agendamentos> Agendamentos { get; set; }
    }
}
