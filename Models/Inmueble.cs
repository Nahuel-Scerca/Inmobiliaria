using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplicationPrueba.Models
{
    public enum enEstado
    {
        Disponible = 1,
        No_disponible = 2,
        En_Refaccion = 3,
    }
    public class Inmueble
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public string Uso { get; set; }
        [Required]
        public int Superficie { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public int Estado { get; set; }
        //[Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        //[ForeignKey("PropietarioId")]
        public Propietario Duenio { get; set; }

        public string Foto { get; set; }


        [Display(Name = "Estado")]
        public string EstadoNombre => Estado > 0 ? ((enEstado)Estado).ToString().Replace('_', ' ') : "";

        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> estados = new SortedDictionary<int, string>();
            Type tipoEnumEstado = typeof(enEstado);
            foreach (var valor in Enum.GetValues(tipoEnumEstado))
            {
                estados.Add((int)valor, Enum.GetName(tipoEnumEstado, valor).Replace('_',' '));
            }
            return estados;
        }

    }
}
