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
	public class DocumentoSolicitud
	{
		[Key]
		public Int32 IdDocumento { get; set; }
		public Int32 IdSolicitud { get; set; }
		public String NombreDocumento { get; set; }
		public DateTime FechaAgregado { get; set; }
		public Int32 IdUsuario { get; set; }
		public String TipoDocumento { get; set; }

		public DocumentoSolicitud()
		{
			IdDocumento = 0;
			IdSolicitud = 0;
			NombreDocumento = string.Empty;
			FechaAgregado = DateTime.Now;
			IdUsuario = 0;
			TipoDocumento = string.Empty;
		}

		public DocumentoSolicitud(Decimal ID)
		{
			DataTable dtc = DataAccess.GetValuesInDataTable("Select * from DocumentoSolicitud WHERE IdDocumento = " + ID);
			if (dtc.Rows.Count > 0)
			{
				IdDocumento = Convert.ToInt32(dtc.Rows[0]["IdDocumento"]);
				IdSolicitud = Convert.ToInt32(dtc.Rows[0]["IdSolicitud"]);
				NombreDocumento = Convert.ToString(dtc.Rows[0]["NombreDocumento"]);
				FechaAgregado = Convert.ToDateTime(dtc.Rows[0]["FechaAgregado"]);
				IdUsuario = Convert.ToInt32(dtc.Rows[0]["IdUsuario"]);
				TipoDocumento = Convert.ToString(dtc.Rows[0]["TipoDocumento"]);
			}
		}
		public static List<DocumentoSolicitud> DocumentoSolicitudList(string sql = "select * from DocumentoSolicitud")
		{
			List<DocumentoSolicitud> List = new List<DocumentoSolicitud>();
			DataTable dt = new DataTable();
			DocumentoSolicitud entidad;
			dt = DataAccess.GetValuesInDataTable(sql);
			foreach (DataRow item in dt.Rows)
			{
				entidad = new DocumentoSolicitud();
				entidad.IdDocumento = Convert.ToInt32(item["IdDocumento"]);
				entidad.IdSolicitud = Convert.ToInt32(item["IdSolicitud"]);
				entidad.NombreDocumento = Convert.ToString(item["NombreDocumento"]);
				entidad.FechaAgregado = Convert.ToDateTime(item["FechaAgregado"]);
				entidad.IdUsuario = Convert.ToInt32(item["IdUsuario"]);
				entidad.TipoDocumento = Convert.ToString(item["TipoDocumento"]);
				List.Add(entidad);
			}
			return List;
		}

		public bool Guardar()
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPSaveDocumentoSolicitud";
			cmd.Parameters.AddWithValue("@IdDocumento", IdDocumento);
			cmd.Parameters.AddWithValue("@IdSolicitud", IdSolicitud);
			cmd.Parameters.AddWithValue("@NombreDocumento", NombreDocumento);
			cmd.Parameters.AddWithValue("@FechaAgregado", FechaAgregado);
			cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
			cmd.Parameters.AddWithValue("@TipoDocumento", TipoDocumento);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output;

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}


		public bool Eliminar()
		{

			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPDeleteDocumentoSolicitud";
			cmd.Parameters.AddWithValue("@IdDocumento", IdDocumento);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output; 

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}
	}
}