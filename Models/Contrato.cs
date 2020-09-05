using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationPrueba.Models
{
    public class Contrato
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Fecha Emision")]
        public DateTime FechaDesde { get; set; }

        [Required]
        [Display(Name = "Fecha Vencimiento")]
        public DateTime FechaHasta { get; set; }
        

        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }
        [ForeignKey("InquilinoId")]
        public Inquilino Inquilino { get; set; }


        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }
        [ForeignKey("InmuebleId")]
        public Inmueble Inmueble { get; set; }
    }
}
