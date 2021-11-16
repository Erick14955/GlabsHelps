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
	public class Usuario
	{
		[Key]
		public Int32 IdUsuario { get; set; }
		public String NombreUsuario { get; set; }
		public String CorreoElectronico { get; set; }
		public String Password { get; set; }

		public Usuario()
		{
			IdUsuario = 0;
			NombreUsuario = string.Empty;
			CorreoElectronico = string.Empty;
			Password = string.Empty;
		}
		public Usuario(Decimal ID)
		{
			DataTable dtc = DataAccess.GetValuesInDataTable("Select * from Usuario WHERE IdUsuario = " + ID);
			if (dtc.Rows.Count > 0)
			{
				IdUsuario = Convert.ToInt32(dtc.Rows[0]["IdUsuario"]);
				NombreUsuario = Convert.ToString(dtc.Rows[0]["NombreUsuario"]);
				CorreoElectronico = Convert.ToString(dtc.Rows[0]["CorreoElectronico"]);
				Password = Convert.ToString(dtc.Rows[0]["Password"]);
			}
		}

		public static List<Usuario> UsuarioList(string sql = "select * from Usuario")
		{
			List<Usuario> List = new List<Usuario>();
			DataTable dt = new DataTable();
			Usuario entidad;
			dt = DataAccess.GetValuesInDataTable(sql);
			foreach (DataRow item in dt.Rows)
			{
				entidad = new Usuario();
				entidad.IdUsuario = Convert.ToInt32(item["IdUsuario"]);
				entidad.NombreUsuario = Convert.ToString(item["NombreUsuario"]);
				entidad.CorreoElectronico = Convert.ToString(item["CorreoElectronico"]);
				entidad.Password = Convert.ToString(item["Password"]);
				List.Add(entidad);
			}
			return List;
		}

		public bool Guardar()
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPSaveUsuario";
			cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
			cmd.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
			cmd.Parameters.AddWithValue("@CorreoElectronico", CorreoElectronico);
			cmd.Parameters.AddWithValue("@Password", Password);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output; 

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}


		public bool Eliminar()
		{

			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SPDeleteUsuario";
			cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
			cmd.Parameters.Add("@ValorRetorno", SqlDbType.Int);
			cmd.Parameters["@ValorRetorno"].Direction = ParameterDirection.Output; 

			return DataAccess.SaveSPIntValueReturn("@ValorRetorno", ref cmd);

		}
	}

}