using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace WebApplicationPrueba.Models
{
    public class Pago
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Monto")]
        public SqlMoney Monto { get; set; }



        [Display(Name = "Contrato")]
        public int ContratoId { get; set; }
        [ForeignKey("ContratoId")]
        public Contrato Contrato { get; set; }
    }
}
