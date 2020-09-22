using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPrueba.Models
{
	public class RepositorioPago : RepositorioBase, IRepositorioPago
	{

		public RepositorioPago(IConfiguration configuration) : base(configuration)
		{

		}



		public int Alta(Pago pago)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO pagos (Fecha, Monto, ContratoId) " +
					"VALUES (@Fecha, @Monto, @ContratoId);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Fecha", pago.Fecha);
					command.Parameters.AddWithValue("@Monto", pago.Monto);
					command.Parameters.AddWithValue("@ContratoId", pago.ContratoId);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					pago.Id = res;
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
				string sql = $"DELETE FROM pagos WHERE Id = {id}";
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


		public int Modificacion(Pago pago)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE pagos SET " +
					"Fecha=@Fecha, Monto=@Monto, ContratoId=@ContratoId " +
					"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@Fecha", pago.Fecha);
					command.Parameters.AddWithValue("@Monto", pago.Monto);
					command.Parameters.AddWithValue("@ContratoId", pago.ContratoId);

					command.Parameters.AddWithValue("@id", pago.Id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}


		public IList<Pago> ObtenerTodos()
		{
			IList<Pago> res = new List<Pago>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{

				//sql funcional
				string consultasql = "SELECT c.Id, FechaDesde, FechaHasta, InquilinoId, InmuebleId," +
					" i.Nombre AS InquilinoNombre,i.Apellido AS InquilinoApellido," +
					"inm.Direccion, inm.PropietarioId,p.Nombre AS PropietarioNombre ," +
					 "p.Apellido AS PropietarioApellido ," +
					 "pa.id AS PagoId,pa.fecha AS fechaPago ," +
					 "pa.monto AS montoPago,pa.contratoId" +
					 " FROM Contratos c " +
					 " INNER JOIN Inquilinos i ON i.Id = c.InquilinoId" +
					 " INNER JOIN Inmuebles inm ON inm.Id = c.InmuebleId" +
					 " INNER JOIN Propietarios p ON inm.PropietarioId = p.Id" +
					 " INNER JOIN Pagos pa ON pa.ContratoId = c.Id";


				using (SqlCommand command = new SqlCommand(consultasql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago pago = new Pago
						{
							Id = reader.GetInt32(11),
							Fecha = reader.GetDateTime(12),
							Monto = reader.GetSqlMoney(13),
							ContratoId = reader.GetInt32(0),

							Contrato = new Contrato
							{
								Id = reader.GetInt32(0),
								FechaDesde = reader.GetDateTime(1),
								FechaHasta = reader.GetDateTime(2),
								InquilinoId = reader.GetInt32(3),
								InmuebleId = reader.GetInt32(4),

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
							},




						};
						res.Add(pago);
					}
					connection.Close();
				}
			}
			return res;
		}




		public Pago ObtenerPorId(int id)
		{
			Pago pago = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{


				string sql = "SELECT pa.Id,pa.monto,pa.fecha,pa.contratoId," +
					"c.FechaDesde, c.FechaHasta, c.InquilinoId, c.InmuebleId," +
					" i.Nombre,i.Apellido," +
					"inm.Direccion, inm.PropietarioId," +
					"p.Nombre,p.Apellido" +
					 " FROM Pagos pa " +
					 "INNER JOIN Contratos c ON Pa.contratoId = c.Id" +
					 " INNER JOIN Inquilinos i ON i.Id = c.InquilinoId" +
					 " INNER JOIN Inmuebles inm ON inm.Id = c.InmuebleId" +
					 " INNER JOIN Propietarios p ON inm.PropietarioId = p.Id" +
					 $" WHERE pa.Id=@id ";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						pago = new Pago
						{
							Id = reader.GetInt32(0),
							Fecha = reader.GetDateTime(2),
							Monto = reader.GetSqlMoney(1),
							ContratoId = reader.GetInt32(3),



							Contrato = new Contrato
							{
								Id = reader.GetInt32(0),
								FechaDesde = reader.GetDateTime(4),
								FechaHasta = reader.GetDateTime(5),
								InquilinoId = reader.GetInt32(6),
								InmuebleId = reader.GetInt32(7),

								Inquilino = new Inquilino
								{
									Id = reader.GetInt32(6),
									Nombre = reader.GetString(8),
									Apellido = reader.GetString(9),
								},

								Inmueble = new Inmueble
								{
									Id = reader.GetInt32(7),
									Direccion = reader.GetString(10),
									PropietarioId = reader.GetInt32(11),

									Duenio = new Propietario
									{
										Id = reader.GetInt32(11),
										Nombre = reader.GetString(12),
										Apellido = reader.GetString(13),
									}
								}
							}
						};
						connection.Close();
					}
				}
				return pago;
			}

		}
	}
}
