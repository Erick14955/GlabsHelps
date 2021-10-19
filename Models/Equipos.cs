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
	public class Equipos
	{
		[Key]
		public Int32 IdEquipo { get; set; }
		public Int32 IdCliente { get; set; }
		[Required]
		public String Descripcion { get; set; }
		[Required]
		public String Responsable { get; set; }
		[Required]
		public String DireccionAnyDesk { get; set; }
		[Required]
		public String DireccionTeamViewer { get; set; }
		public String IpEquipo { get; set; }
		public String IpPublica { get; set; }
		public String IpLocal { get; set; }

		public String TipoEquipo { get; set; }
		public String UsuarioEquipo { get; set; }
		public String ClaveEquipo { get; set; }

		public Equipos()
		{
			IdEquipo = 0;
			IdCliente = 0;
			Descripcion = string.Empty;
			Responsable = string.Empty;
			DireccionAnyDesk = string.Empty;
			DireccionTeamViewer = string.Empty;
			IpEquipo = string.Empty;
			IpPublica = string.Empty;
			IpLocal = string.Empty;
			TipoEquipo = string.Empty;
			UsuarioEquipo = string.Empty;
			ClaveEquipo = string.Empty;
		}

		public Equipos(Decimal ID)
		{
			DataTable dtc = DataAccess.GetValuesInDataTable("Select * from Equipo WHERE IdEquipo = " + ID);
			if (dtc.Rows.Count > 0)
			{
				IdEquipo = Convert.ToInt32(dtc.Rows[0]["IdEquipo"]);
				IdCliente = Convert.ToInt32(dtc.Rows[0]["IdCliente"]);
				Descripcion = Convert.ToString(dtc.Rows[0]["Descripcion"]);
				Responsable = Convert.ToString(dtc.Rows[0]["Responsable"]);
				DireccionAnyDesk = Convert.ToString(dtc.Rows[0]["DireccionAnyDesk"]);
				DireccionTeamViewer = Convert.ToString(dtc.Rows[0]["DireccionTeamViewer"]);
				IpEquipo = Convert.ToString(dtc.Rows[0]["IpEquipo"]);
				IpPublica = Convert.ToString(dtc.Rows[0]["IpPublica"]);
				IpLocal = Convert.ToString(dtc.Rows[0]["IpLocal"]);
				TipoEquipo = Convert.ToString(dtc.Rows[0]["TipoEquipo"]);
				UsuarioEquipo = Convert.ToString(dtc.Rows[0]["UsuarioEquipo"]);
				ClaveEquipo = Convert.ToString(dtc.Rows[0]["ClaveEquipo"]);
			}
		}

		public static List<Equipos> EquipoList(string sql = "select * from Equipo")
		{
			List<Equipos> List = new List<Equipos>();
			DataTable dt = new DataTable();
			Equipos entidad;
			dt = DataAccess.GetValuesInDataTable(sql);
			foreach (DataRow item in dt.Rows)
			{
				entidad = new Equipos();
				entidad.IdEquipo = Convert.ToInt32(item["IdEquipo"]);
				entidad.IdCliente = Convert.ToInt32(item["IdCliente"]);
				entidad.Descripcion = Convert.ToString(item["Descripcion"]);
				entidad.Responsable = Convert.ToString(item["Responsable"]);
				entidad.DireccionAnyDesk = Convert.ToString(item["DireccionAnyDesk"]);
				entidad.DireccionTeamViewer = Convert.ToString(item["DireccionTeamViewer"]);
				entidad.IpEquipo = Convert.ToString(item["IpEquipo"]);
				entidad.IpPublica = Convert.ToString(item["IpPublica"]);
				entidad.IpLocal = Convert.ToString(item["IpLocal"]);
				entidad.TipoEquipo = Convert.ToString(item["TipoEquipo"]);
				entidad.UsuarioEquipo = Convert.ToString(item["UsuarioEquipo"]);
				entidad.ClaveEquipo = Convert.ToString(item["ClaveEquipo"]);
				List.Add(entidad);
			}
			return List;
		}


		public bool Guardar()
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPSaveEquipo";
			cmd.Parameters.AddWithValue("@IdEquipo", IdEquipo);
			cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
			cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
			cmd.Parameters.AddWithValue("@Responsable", Responsable);
			cmd.Parameters.AddWithValue("@DireccionAnyDesk", DireccionAnyDesk);
			cmd.Parameters.AddWithValue("@DireccionTeamViewer", DireccionTeamViewer);
			cmd.Parameters.AddWithValue("@IpEquipo", IpEquipo);
			cmd.Parameters.AddWithValue("@IpPublica", IpPublica);
			cmd.Parameters.AddWithValue("@IpLocal", IpLocal);
			cmd.Parameters.AddWithValue("@TipoEquipo", TipoEquipo);
			cmd.Parameters.AddWithValue("@UsuarioEquipo", UsuarioEquipo);
			cmd.Parameters.AddWithValue("@ClaveEquipo", ClaveEquipo);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output; //se debe especificar que es output

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}


		public bool Eliminar()
		{

			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPDeleteEquipo";
			cmd.Parameters.AddWithValue("@IdEquipo", IdEquipo);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output; //se debe especificar que es output

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}
	}
}