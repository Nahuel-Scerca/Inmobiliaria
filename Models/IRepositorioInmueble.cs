using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPrueba.Models
{
	public interface IRepositorioInmueble : IRepositorio<Inmueble>
	{
		IList<Inmueble> BuscarPorPropietario(int idPropietario);
		IList<Inmueble> ObtenerTodos(Boolean estado);
	}
}
