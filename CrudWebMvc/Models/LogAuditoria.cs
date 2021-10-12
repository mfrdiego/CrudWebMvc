using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CrudWebMvc.Models
{

    [Table("LogAuditoria")]
    public class LogAuditoria
    {
        [Column("Id")]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Column("DetalhesAuditoria")]
        [Display(Name = "Detalhes Auditoria")]
        public String DetalhesAuditoria{ get; set; }

        [Column("EmailUsuario")]
        [Display(Name = "Email Usuário")]

        public String EmailUsuario { get; set; }
    }
}
