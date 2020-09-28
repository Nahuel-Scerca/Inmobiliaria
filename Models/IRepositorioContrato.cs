using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPrueba.Models
{
	public interface IRepositorioContrato : IRepositorio<Contrato>
	{
		// agregar clases queq uiera

		public IList<Contrato> ObtenerPorFecha(DateTime desde, DateTime hasta);

	}
}
