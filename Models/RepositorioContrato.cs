using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPrueba.Models
{
    public class RepositorioContrato : RepositorioBase , IRepositorioContrato
	{

        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
      
        }



		public int Alta(Contrato contrato)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Contratos (FechaDesde,FechaHasta, InquilinoId, InmuebleId, PrecioMensual) " +
					"VALUES (@FechaDesde, @FechaHasta, @InquilinoId, @InmuebleId, @PrecioMensual);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@FechaDesde", contrato.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", contrato.FechaHasta);
					command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
					command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
					command.Parameters.AddWithValue("@PrecioMensual", contrato.PrecioMensual);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					contrato.Id = res;
					connection.Close();
				}
			}
			return res;
		}


		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Contratos WHERE Id = {id}";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}


		public int Modificacion(Contrato contrato)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Contratos SET " +
					"FechaDesde=@FechaDesde, FechaHasta=@FechaHasta, InquilinoId=@InquilinoId, InmuebleId=@InmuebleId, PrecioMensual=@PrecioMensual " +
					"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@FechaDesde", contrato.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", contrato.FechaHasta);
					command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
					command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
					command.Parameters.AddWithValue("@PrecioMensual", contrato.PrecioMensual);

					command.Parameters.AddWithValue("@id", contrato.Id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}


		public IList<Contrato> ObtenerTodos(int id)
		{
			IList<Contrato> res = new List<Contrato>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{	
				
				//sql funcional
				string consultasql = "SELECT c.Id, FechaDesde, FechaHasta, InquilinoId, InmuebleId,"+
					" i.Nombre AS InquilinoNombre,i.Apellido AS InquilinoApellido," +
					"inm.Direccion, inm.PropietarioId,p.Nombre AS PropietarioNombre ,"+
					 "p.Apellido AS PropietarioApellido , c.PrecioMensual" +
					 " FROM Contratos c " +
					 " INNER JOIN Inquilinos i ON i.Id = c.InquilinoId"+
					 " INNER JOIN Inmuebles inm ON inm.Id = c.InmuebleId"+
					 " INNER JOIN Propietarios p ON inm.PropietarioId = p.Id";
				if (id>0)
				{
					consultasql = consultasql + " WHERE c.InmuebleId=@id";
				}

				using (SqlCommand command = new SqlCommand(consultasql, connection))
				{
                    if (id > 0)
                    {
						command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					}
					
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato contrato = new Contrato
						{
							Id = reader.GetInt32(0),
							FechaDesde = reader.GetDateTime(1),
							FechaHasta = reader.GetDateTime(2),
							InquilinoId = reader.GetInt32(3),
							InmuebleId = reader.GetInt32(4),
							PrecioMensual = reader.GetDecimal(11),

							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(3),
								Nombre = reader.GetString(5),
								Apellido = reader.GetString(6),
							},

							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(4),
								Direccion = reader.GetString(7),
								PropietarioId = reader.GetInt32(8),
								
								Duenio = new Propietario
                                {
									Id = reader.GetInt32(8),
									Nombre = reader.GetString(9),
									Apellido = reader.GetString(10),
								}
							}
						};
						res.Add(contrato);
					}
					connection.Close();
				}
			}
			return res;
		}




		public Contrato ObtenerPorId(int id)
		{
			Contrato contrato = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT c.Id, FechaDesde, FechaHasta, InquilinoId, InmuebleId," +
					" i.Nombre AS InquilinoNombre,i.Apellido AS InquilinoApellido," +
					"inm.Direccion, inm.PropietarioId,p.Nombre AS PropietarioNombre ," +
					 "p.Apellido AS PropietarioApellido , PrecioMensual" +
					 " FROM Contratos c " +
					 " INNER JOIN Inquilinos i ON i.Id = c.InquilinoId" +
					 " INNER JOIN Inmuebles inm ON inm.Id = c.InmuebleId" +
					 " INNER JOIN Propietarios p ON inm.PropietarioId = p.Id"+
					 $" WHERE c.Id=@id ";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						contrato = new Contrato
						{
							Id = reader.GetInt32(0),
							FechaDesde = reader.GetDateTime(1),
							FechaHasta = reader.GetDateTime(2),
							InquilinoId = reader.GetInt32(3),
							InmuebleId = reader.GetInt32(4),
							PrecioMensual = reader.GetDecimal(11),

							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(3),
								Nombre = reader.GetString(5),
								Apellido = reader.GetString(6),
							},

							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(4),
								Direccion = reader.GetString(7),
								PropietarioId = reader.GetInt32(8),

								Duenio = new Propietario
								{
									Id = reader.GetInt32(8),
									Nombre = reader.GetString(9),
									Apellido = reader.GetString(10),
								}
							}
						};
					}
					connection.Close();
				}
			}
			return contrato;
		}



        public IList<Contrato> ObtenerPorFecha(DateTime desde, DateTime hasta)
        {
			IList<Contrato> res = new List<Contrato>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{

				//sql funcional
				string consultasql = "SELECT c.Id, FechaDesde, FechaHasta, InquilinoId, InmuebleId," +
					" i.Nombre AS InquilinoNombre,i.Apellido AS InquilinoApellido," +
					"inm.Direccion, inm.PropietarioId,p.Nombre AS PropietarioNombre ," +
					 "p.Apellido AS PropietarioApellido , c.PrecioMensual" +
					 " FROM Contratos c " +
					 " INNER JOIN Inquilinos i ON i.Id = c.InquilinoId" +
					 " INNER JOIN Inmuebles inm ON inm.Id = c.InmuebleId" +
					 " INNER JOIN Propietarios p ON inm.PropietarioId = p.Id "+
					 " WHERE "+
					 "CAST(c.FechaDesde AS datetime) >  CAST(@desde AS datetime)"+
					 " AND CAST(c.FechaHasta AS datetime) < CAST(@hasta AS datetime)";

				using (SqlCommand command = new SqlCommand(consultasql, connection))
				{
					command.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
					command.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato contrato = new Contrato
						{
							Id = reader.GetInt32(0),
							FechaDesde = reader.GetDateTime(1),
							FechaHasta = reader.GetDateTime(2),
							InquilinoId = reader.GetInt32(3),
							InmuebleId = reader.GetInt32(4),
							PrecioMensual = reader.GetDecimal(11),

							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(3),
								Nombre = reader.GetString(5),
								Apellido = reader.GetString(6),
							},

							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(4),
								Direccion = reader.GetString(7),
								PropietarioId = reader.GetInt32(8),

								Duenio = new Propietario
								{
									Id = reader.GetInt32(8),
									Nombre = reader.GetString(9),
									Apellido = reader.GetString(10),
								}
							}
						};
						res.Add(contrato);
					}
					connection.Close();
				}
			}
			return res;
		}

        public IList<Contrato> ObtenerTodos()
        {
			return ObtenerTodos(0);
		}
    }
}
