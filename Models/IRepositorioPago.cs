﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPrueba.Models
{
	public interface IRepositorioPago : IRepositorio<Pago>
	{
		//Agregar clases aqui
			IList<Pago> BuscarPorContrato(int id);
	}
}
