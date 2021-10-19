using GlabsHelps.Models.ClasesUtilitarias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GlabsHelps.Models
{
	public class Clientes
	{
		[Key]
		public Int32 IdCliente { get; set; }
		public String Nombre { get; set; }
		public String Direccion { get; set; }
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Favor especificar un teléfono válido.")]
		[Required(ErrorMessage = "Favor especificar el teléfono del empleado.")]
		[Display(Name = "Teléfono")]
		public String Telefono { get; set; }
		[DataType(DataType.EmailAddress)]
		[EmailAddress(ErrorMessage = "Favor especificar una dirección correo válida.")]
		public String CorreoCliente { get; set; }
		public String Contacto { get; set; }
		public String CelularContacto { get; set; }
		[DataType(DataType.EmailAddress)]
		[EmailAddress(ErrorMessage = "Favor especificar una dirección correo válida.")]
		public String CorreoContacto { get; set; }

		public Clientes()
		{
			IdCliente = 0;
			Nombre = string.Empty;
			Direccion = string.Empty;
			Telefono = string.Empty;
			CorreoCliente = string.Empty;
			Contacto = string.Empty;
			CelularContacto = string.Empty;
			CorreoContacto = string.Empty;
		}

		public Clientes(Decimal ID)
		{
			DataTable dtc = DataAccess.GetValuesInDataTable("Select * from Cliente WHERE IdCliente = " + ID);
			if (dtc.Rows.Count > 0)
			{
				IdCliente = Convert.ToInt32(dtc.Rows[0]["IdCliente"]);
				Nombre = Convert.ToString(dtc.Rows[0]["Nombre"]);
				Direccion = Convert.ToString(dtc.Rows[0]["Direccion"]);
				Telefono = Convert.ToString(dtc.Rows[0]["Telefono"]);
				CorreoCliente = Convert.ToString(dtc.Rows[0]["CorreoCliente"]);
				Contacto = Convert.ToString(dtc.Rows[0]["Contacto"]);
				CelularContacto = Convert.ToString(dtc.Rows[0]["CelularContacto"]);
				CorreoContacto = Convert.ToString(dtc.Rows[0]["CorreoContacto"]);
			}
		}

		public static List<Clientes> ClienteList(string sql = "select * from Cliente")
		{
			List<Clientes> List = new List<Clientes>();
			DataTable dt = new DataTable();
			Clientes entidad;
			dt = DataAccess.GetValuesInDataTable(sql);
			foreach (DataRow item in dt.Rows)
			{
				entidad = new Clientes();
				entidad.IdCliente = Convert.ToInt32(item["IdCliente"]);
				entidad.Nombre = Convert.ToString(item["Nombre"]);
				entidad.Direccion = Convert.ToString(item["Direccion"]);
				entidad.Telefono = Convert.ToString(item["Telefono"]);
				entidad.CorreoCliente = Convert.ToString(item["CorreoCliente"]);
				entidad.Contacto = Convert.ToString(item["Contacto"]);
				entidad.CelularContacto = Convert.ToString(item["CelularContacto"]);
				entidad.CorreoContacto = Convert.ToString(item["CorreoContacto"]);
				List.Add(entidad);
			}
			return List;
		}

		public bool Guardar()
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPSaveCliente";
			cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
			cmd.Parameters.AddWithValue("@Nombre", Nombre);
			cmd.Parameters.AddWithValue("@Direccion", Direccion);
			cmd.Parameters.AddWithValue("@Telefono", Telefono);
			cmd.Parameters.AddWithValue("@CorreoCliente", CorreoCliente);
			cmd.Parameters.AddWithValue("@Contacto", Contacto);
			cmd.Parameters.AddWithValue("@CelularContacto", CelularContacto);
			cmd.Parameters.AddWithValue("@CorreoContacto", CorreoContacto);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output; //se debe especificar que es output

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}


		public bool Eliminar()
		{

			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPDeleteCliente";
			cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output; //se debe especificar que es output

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}
	}
}