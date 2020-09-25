using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPrueba.Models
{
    public class Inquilino
    {
			[Key]
			[Display(Name = "Código")]
			public int Id { get; set; }
			[Required]
			public string Nombre { get; set; }
			[Required]
			public string Apellido { get; set; }
			[Required]
			public string Dni { get; set; }
			public string Telefono { get; set; }
			[Required, EmailAddress]
			public string Email { get; set; }


			[Required]
			[Display(Name = "Garante")]
			public string NombreGarante { get; set; }
			[Required]
			[Display(Name = "DNI Garante")]
			public string DniGarante { get; set; }
			[Required]
			[Display(Name = "Telefono Garante")]
			public string TelefonoGarante { get; set; }
	}
}
