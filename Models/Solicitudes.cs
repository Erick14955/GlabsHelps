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
	public class Solicitudes
	{
		[Key]
		public Int32 IdSolicitud { get; set; }
		public String Descripcion { get; set; }
		public String Detalle { get; set; }
		public DateTime Fecha { get; set; }
		public DateTime FechaFinalizado { get; set; }
		public Int32 Estatus { get; set; }
		public Int32 IdCliente { get; set; }
		public Int32 IdUsuario { get; set; }
		public Int32 IdIngeniero { get; set; }
		public DateTime FechaAgenda { get; set; }

		public Solicitudes()
		{
			IdSolicitud = 0;
			Descripcion = string.Empty;
			Detalle = string.Empty;
			Fecha = DateTime.Now;
			FechaFinalizado = DateTime.Now;
			Estatus = 0;
			IdCliente = 0;
			IdUsuario = 0;
			IdIngeniero = 0;
			FechaAgenda = DateTime.Now;

		}

		public Solicitudes(Decimal ID)
		{
			DataTable dtc = DataAccess.GetValuesInDataTable("Select * from Solicitudes WHERE IdSolicitud = " + ID);
			if (dtc.Rows.Count > 0)
			{
				IdSolicitud = Convert.ToInt32(dtc.Rows[0]["IdSolicitud"]);
				Descripcion = Convert.ToString(dtc.Rows[0]["Descripcion"]);
				Detalle = Convert.ToString(dtc.Rows[0]["Detalle"]);
				Fecha = Convert.ToDateTime(dtc.Rows[0]["Fecha"]);
				FechaFinalizado = Convert.ToDateTime(dtc.Rows[0]["FechaFinalizado"]);
				Estatus = Convert.ToInt32(dtc.Rows[0]["Estatus"]);
				IdCliente = Convert.ToInt32(dtc.Rows[0]["IdCliente"]);
				IdUsuario = Convert.ToInt32(dtc.Rows[0]["IdUsuario"]);
				IdIngeniero = Convert.ToInt32(dtc.Rows[0]["IdIngeniero"]);
				FechaAgenda = Convert.ToDateTime(dtc.Rows[0]["FechaAgenda"]);
			}
		}

		public static List<Solicitudes> SolicitudesList(string sql = "select * from Solicitudes")
		{
			List<Solicitudes> List = new List<Solicitudes>();
			DataTable dt = new DataTable();
			Solicitudes entidad;
			dt = DataAccess.GetValuesInDataTable(sql);
			foreach (DataRow item in dt.Rows)
			{
				entidad = new Solicitudes();
				entidad.IdSolicitud = Convert.ToInt32(item["IdSolicitud"]);
				entidad.Descripcion = Convert.ToString(item["Descripcion"]);
				entidad.Detalle = Convert.ToString(item["Detalle"]);
				entidad.Fecha = Convert.ToDateTime(item["Fecha"]);
				entidad.FechaFinalizado = Convert.ToDateTime(item["FechaFinalizado"]);
				entidad.Estatus = Convert.ToInt32(item["Estatus"]);
				entidad.IdCliente = Convert.ToInt32(item["IdCliente"]);
				entidad.IdUsuario = Convert.ToInt32(item["IdUsuario"]);
				entidad.IdIngeniero = Convert.ToInt32(item["IdIngeniero"]);
				entidad.FechaAgenda = Convert.ToDateTime(item["FechaAgenda"]);
				List.Add(entidad);
			}
			return List;
		}

		public bool Guardar()
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPSaveSolicitudes";
			cmd.Parameters.AddWithValue("@IdSolicitud", IdSolicitud);
			cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
			cmd.Parameters.AddWithValue("@Detalle", Detalle);
			cmd.Parameters.AddWithValue("@Fecha", Fecha);
			cmd.Parameters.AddWithValue("@FechaFinalizado", FechaFinalizado);
			cmd.Parameters.AddWithValue("@Estatus", Estatus);
			cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
			cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
			cmd.Parameters.AddWithValue("@IdIngeniero", IdIngeniero);
			cmd.Parameters.AddWithValue("@FechaAgenda", FechaAgenda);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output;

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}


		public bool Eliminar()
		{

			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPDeleteSolicitudes";
			cmd.Parameters.AddWithValue("@IdSolicitud", IdSolicitud);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output;

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}
	}
}
