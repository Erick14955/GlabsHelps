using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace GlabsHelps.Models.ClasesUtilitarias
{
    public class Utilidades
    {
        ///// <summary>
        ///// Obtiene un filtro de las empresas permitidas de un usuario para agregar a un WHERE de SQL
        ///// </summary>
        ///// <param name="usuario">Usuario con cadena de empresas a extraer, las cuales deben de estar en Ids separadas por comas ",".</param>
        ///// <returns>Cadena de condicion de busqueda para un WHERE de SQL. Ej: "IdEmpresa = 0 OR IdEmpresa = 2 OR IdEmpresa = 1..."</returns>
        //public static string GetFiltroEmpresas(Usuarios usuario)
        //{
        //    return GetFiltroEmpresas(usuario.EmpresasPermitidas);
        //}

        ///// <summary>
        ///// Obtiene un filtro de idEmpresa para agregar a un WHERE de SQL desde una cadena de Ids de empresas
        ///// </summary>
        ///// <param name="EmpresasPermitidas">Cadena de empresas a extraer, las cuales deben de estar en Ids separadas por comas ",".</param>
        ///// <returns>Cadena de condicion de busqueda para un WHERE de SQL. Ej: "IdEmpresa = 0 OR IdEmpresa = 2 OR IdEmpresa = 1..."</returns>
        //public static string GetFiltroEmpresas(string EmpresasPermitidas)
        //{
        //    if (!string.IsNullOrWhiteSpace(EmpresasPermitidas))
        //    {
        //        string[] idsEmpresasPermitidas = EmpresasPermitidas.Split(',');
        //        string filtroEmpresas = string.Empty;

        //        for (int i = 0; i < idsEmpresasPermitidas.Length; i++)
        //        {
        //            string idEmpresa = idsEmpresasPermitidas[i];
        //            if (i == 0)
        //                filtroEmpresas = string.Format("IdEmpresa = {0}", idEmpresa);
        //            else
        //                filtroEmpresas += string.Format(" OR IdEmpresa = {0}", idEmpresa);
        //        }

        //        if (!string.IsNullOrWhiteSpace(filtroEmpresas))
        //        {
        //            filtroEmpresas = " AND (" + filtroEmpresas + ")";
        //        }
        //        return filtroEmpresas;
        //    }

        //    return EmpresasPermitidas;
        //}

        /// <summary>
        /// Funcion que evalua si un dato es numerico.
        /// </summary>
        /// <param name="Expression"> Objeto a ser evaluado. </param>
        /// <returns>Retorna verdadero o falso. </returns>
        public static bool IsNumeric(object Expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        //public static double TasaActual(int IdMoneda)
        //{
        //    Monedas m = new Monedas(IdMoneda);
        //    return m.Tasa;
        //}

        /// <summary>
        /// Convierte el valor de una columna en string
        /// </summary>
        /// <param name="dataColumn">Valor de la columna que se va a convertir</param>
        /// <param name="defaultValueFor">Tipo de dato de la columna</param>
        /// <returns>Retorna el valor de la columna convertido en string  y si es null retorna el valor por defecto del tipo de dato especificado</returns>
        public static string ColumnToString(object dataColumn, TipoValor defaultValueFor)
        {

            string defaultValue = "";

            switch (defaultValueFor)
            {
                case TipoValor.String:
                    defaultValue = "";
                    break;

                case TipoValor.Numeric:
                    defaultValue = "0";
                    break;

                case TipoValor.Date:
                    defaultValue = DateTime.MinValue.ToString();
                    break;

                case TipoValor.Bool:
                    defaultValue = false.ToString();
                    break;
            }

            bool IsNull = DBNull.Value.Equals(dataColumn);

            if (IsNull)
                return defaultValue;
            else
                return Convert.ToString(dataColumn);
        }

        /// <summary>
        /// Para obtener el string de un DataTable
        /// </summary>
        /// <param name="dt">DataTable de donde se obtendran los valores</param>
        /// <returns>Un string en formato XML con el contenido del DataTable</returns>
        public static string DataTableToXMlString(ref DataTable dt)
        {
            DataSet ds = new DataSet("detalle");  //Crea el dataset 
            ds.Tables.Add(dt); //agrega la tabla al dataset
            dt.TableName = "rows"; //el nombre de la tabla

            ///------------------------------------------
            ///Cambiando el formato del xml a atributos
            ///------------------------------------------
            foreach (DataColumn dc in dt.Columns)
            {
                dc.ColumnMapping = MappingType.Attribute;
            }
            //----------
            ///Para convertir el xml a un string y pasarlo al stored procedure
            ///-------------------------------------------------------------------
            StringWriter writer = new StringWriter();
            ds.WriteXml(writer, XmlWriteMode.IgnoreSchema);
            writer.Close(); //cierrar el writer.
            return writer.ToString();
        }

        public static string GetValuesst(string Query)
        {
            return DataAccess.GetValue(Query);
        }

        public static string GetValuesst(SqlCommand command)
        {
            return DataAccess.GetValue(command);
        }

        /// <summary>
        /// Obtiene la expresion sql server para ejecutar un procedimiento almacenado manualmente. Este metodo es para fines de depuracion.
        /// </summary>
        /// <param name="cmd">SqlCommand que contiene el procedimiento almacenado y sus parametros con valores.</param>
        /// <param name="spName">Nombre del procedimiento almacenado.</param>
        /// <returns></returns>
        public static string GetSPString(SqlCommand cmd, string spName)
        {
            string spString = string.Format("exec {0}", spName);
            for (int i = 0; i < cmd.Parameters.Count; i++)
            {
                try
                {
                    Convert.ToDouble(cmd.Parameters[i].Value);
                    if (cmd.Parameters[i].Value.GetType() == "".GetType() || cmd.Parameters[i].Value.GetType() == true.GetType())
                    {
                        spString += string.Format(" {0} = '{1}',", cmd.Parameters[i], cmd.Parameters[i].Value);
                    }
                    else
                    {
                        spString += string.Format(" {0} = {1},", cmd.Parameters[i], cmd.Parameters[i].Value);
                    }
                }
                catch (Exception)
                {
                    spString += string.Format(" {0} = '{1}',", cmd.Parameters[i], cmd.Parameters[i].Value);
                }

            }

            return spString;
        }

        /// <summary>
        /// Obtiene la expresion sql server para ejecutar un procedimiento almacenado manualmente. Este metodo es para fines de depuracion.
        /// </summary>
        /// <param name="adapter">SqlDataAdapter que contiene el procedimiento almacenado y sus parametros con valores.</param>
        /// <param name="spName">Nombre del procedimiento almacenado.</param>
        /// <returns></returns>
        public static string GetSPString(SqlDataAdapter adapter, string spName)
        {
            string spString = string.Format("exec {0}", spName);
            for (int i = 0; i < adapter.SelectCommand.Parameters.Count; i++)
            {
                try
                {
                    Convert.ToDouble(adapter.SelectCommand.Parameters[i].Value);
                    if (adapter.SelectCommand.Parameters[i].Value.GetType() == "".GetType() || adapter.SelectCommand.Parameters[i].Value.GetType() == true.GetType())
                    {
                        spString += string.Format(" {0} = '{1}',", adapter.SelectCommand.Parameters[i], adapter.SelectCommand.Parameters[i].Value);
                    }
                    else
                    {
                        spString += string.Format(" {0} = {1},", adapter.SelectCommand.Parameters[i], adapter.SelectCommand.Parameters[i].Value);
                    }
                }
                catch (Exception)
                {
                    spString += string.Format(" {0} = '{1}',", adapter.SelectCommand.Parameters[i], adapter.SelectCommand.Parameters[i].Value);
                }

            }

            return spString;
        }

        /// <summary>
        /// Obtiene los campos que invalidan el ModelState. Este metodo es para fines de depuracion.
        /// </summary>
        /// <param name="ModelState">El ModelState a revizar.</param>
        /// <returns>Una cadena con los Values que invalidan el ModelState separados por guiones.</returns>
        public static string GetModelStateInvalidFields(System.Web.Mvc.ModelStateDictionary ModelState)
        {
            string invalidFieldIndexes = "";

            for (int i = 0; i < ModelState.Values.Count; i++)
            {
                var value = ModelState.Values.ElementAt(i);

                if (value.Errors.Count > 0)
                {
                    invalidFieldIndexes += i + "-";
                }
            }

            return invalidFieldIndexes;
        }

        //public static CrystalDecisions.CrystalReports.Engine.ReportDocument ShowReport(CrystalDecisions.CrystalReports.Engine.ReportDocument Rep, string ServerName, string ServerUser, string ServerPwd, string Formula, string Parametros, string DataBase)
        //{
        //    Rep.DataSourceConnections[0].SetConnection(ServerName, DataBase, false);
        //    Rep.DataSourceConnections[0].SetLogon(ServerUser, ServerPwd);
        //    Rep.RecordSelectionFormula = Formula.ToString();

        //    if (Parametros.Length > 0)
        //    {
        //        string[] Param;
        //        Param = Parametros.Split('#');

        //        for (int i = 0; i <= ((Param.Length) - 1); i++)
        //        {
        //            Rep.SetParameterValue(i, Param[i]);
        //        }
        //    }

        //    return Rep;
        //}

        //public static CrystalDecisions.CrystalReports.Engine.ReportDocument ShowReport(CrystalDecisions.CrystalReports.Engine.ReportDocument Rep, string ServerName, string ServerUser, string ServerPwd, string Formula, string Parametros, string DataBase)
        //{
        //    Rep.DataSourceConnections[0].IntegratedSecurity = false;
        //    Rep.DataSourceConnections[0].SetConnection(ServerName, DataBase, ServerUser, ServerPwd);
        //    Rep.SetDatabaseLogon(ServerUser, ServerPwd, ServerName, DataBase, true);
        //    try
        //    {
        //        foreach (CrystalDecisions.CrystalReports.Engine.Table table in Rep.Database.Tables)
        //        {
        //            table.LogOnInfo.TableName = table.Name;
        //            table.LogOnInfo.ConnectionInfo.UserID = ServerUser;
        //            table.LogOnInfo.ConnectionInfo.Password = ServerPwd;
        //            table.LogOnInfo.ConnectionInfo.DatabaseName = DataBase;
        //            table.LogOnInfo.ConnectionInfo.ServerName = ServerName;
        //            table.ApplyLogOnInfo(table.LogOnInfo);

        //            table.Location = DataBase + ".dbo." + table.Name;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    Rep.RecordSelectionFormula = Formula.ToString();
        //    if (Parametros.Length > 0)
        //    {
        //        string[] Param;
        //        Param = Parametros.Split('#');

        //        for (int i = 0; i <= ((Param.Length) - 1); i++)
        //        {
        //            Rep.SetParameterValue(i, Param[i]);
        //        }
        //    }

        //    return Rep;
        //}

        //public static CrystalDecisions.CrystalReports.Engine.ReportDocument ReportPrint(CrystalDecisions.CrystalReports.Engine.ReportDocument RPT, string server, string db, string user, string pwd, string Formula)
        //{
        //    RPT.DataSourceConnections[0].SetConnection(server, db, false);
        //    RPT.DataSourceConnections[0].SetLogon(user, pwd);
        //    RPT.RecordSelectionFormula = Formula.ToString();

        //    return RPT;
        //}

        //public static CrystalDecisions.CrystalReports.Engine.ReportDocument ReportPrint(CrystalDecisions.CrystalReports.Engine.ReportDocument RPT, string server, string db, string user, string pwd)
        //{
        //    RPT.DataSourceConnections[0].SetConnection(server, db, false);
        //    RPT.DataSourceConnections[0].SetLogon(user, pwd);

        //    return RPT;
        //}

        public static DataTable Busqueda(SqlDataAdapter adapter)
        {
            return DataAccess.GetValuesInDataTable(adapter, 0);

        }

        public static DataTable Busqueda(string Query)
        {
            return DataAccess.GetValuesInDataTable(Query, 0);

        }

        //public static DataTable GetValuesInDataTable(string Query, int TipoCone = 0, int time = 30)
        //{
        //    return DataAcces.GetValuesInDataTable(Query, TipoCone, time);
        //}

        public static DataTable GetValuesInDataTable(SqlDataAdapter adapter, int TipoCone = 0)
        {
            return DataAccess.GetValuesInDataTable(adapter, TipoCone);
        }

        public static double GetDoubleValue(string Query)
        {
            return DataAccess.GetDoubleValue(Query);
        }

        public static double GetDoubleValue(SqlCommand command)
        {
            return DataAccess.GetDoubleValue(command);
        }

        public static Boolean ComandExcute(string Query, int TipoCone = 0, int time = 30)
        {
            return DataAccess.ExQuery(Query, TipoCone, time);
        }

        public static Boolean ComandExcute(SqlCommand command, int TipoCone = 0, int time = 30)
        {
            return DataAccess.ExQuery(command, TipoCone, time);
        }

        /// Generates a hash for the given plain text value and returns a
        /// base64-encoded result. Before the hash is computed, a random salt
        /// is generated and appended to the plain text. This salt is stored at
        /// the end of the hash value, so it can be used later for hash
        /// verification.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be hashed. The function does not check whether
        /// this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1",
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="saltBytes">
        /// Salt bytes. This parameter can be null, in which case a random salt
        /// value will be generated.
        /// </param>
        /// <returns>
        /// Hash value formatted as a base64-encoded string.
        /// </returns>
        public static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash
        /// value. Plain text is hashed with the same salt value as the original
        /// hash.
        /// </summary>
        /// <param name="plainText">
        /// Plain text to be verified against the specified hash. The function
        /// does not check whether this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="hashValue">
        /// Base64-encoded hash value produced by ComputeHash function. This value
        /// includes the original salt appended to it.
        /// </param>
        /// <returns>
        /// If computed hash mathes the specified hash the function the return
        /// value is true; otherwise, the function returns false.
        /// </returns>
        public static bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;

            // Make sure that hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hashSizeInBits = 160;
                    break;

                case "SHA256":
                    hashSizeInBits = 256;
                    break;

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString =
                        ComputeHash(plainText, hashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }

        public static bool ValidarCedula(string cedula)
        {
            bool r = false;
            int[] arr = { 1, 2, 1, 0, 2, 1, 2, 1, 2, 1, 2, 0, 0 };
            cedula = cedula.Replace('-', '0');
            int ultimoDigito = (int)Char.GetNumericValue(cedula[12]);

            int suma = 0;
            int valor;
            for (int i = 0; i < 13; i++)
            {
                valor = 0;
                valor = arr[i] * (int)Char.GetNumericValue(cedula[i]);

                if (valor >= 10)
                {
                    suma += valor - 9;
                }
                else
                {
                    suma += valor;
                }
            }
            if ((suma * 9) % 10 == ultimoDigito)
            {
                return true;
            }
            return r;
        }

        public static bool RNCValido(string rnc)
        {

            bool respuesta = true;

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT empresa FROM RNC WHERE rnc=@rnc", new SqlConnection());
            adapter.SelectCommand.Parameters.AddWithValue("@rnc", rnc.Replace("-", "").Trim());

            DataTable dt = GetValuesInDataTable(adapter);

            if (dt.Rows.Count == 0)
            {
                respuesta = false;
            }

            return respuesta;

        }

        //public static Double RetencionISRVentas(int nfactura)
        //{
        //    EFactura f = new EFactura(nfactura);
        //    NCF ncf = new NCF(f.IDNCF);
        //    if (ncf.Clasificacion == byte.Parse("15"))
        //    {
        //        return (f.Total - f.Impuesto) * GetParameter("RetencionISRGubernamental");
        //    }
        //    return 0;
        //}

        public static Double GetParameter(String parametro)
        {
            return GetDoubleValue("Select " + parametro + " from parametros");
        }

        public static string GetParameter(int nParametro)
        {
            string AbsoluteFilePath = Path.Combine(HttpContext.Current.Server.MapPath("/App_Start/Config.txt"));
            string cadena = Utilidades.GetDataFromFile(AbsoluteFilePath);
            string[] parametros = cadena.Split(Convert.ToChar(";"));
            return parametros[nParametro];
        }

        /// <summary>
        /// Funcion para obtener el contenido de un archivo de texto.
        /// </summary>
        /// <param name="archivo">Ruta del Archivo a Leer.</param>
        /// <returns> string con el contenido del archivo.</returns>
        public static string GetDataFromFile(string archivo)
        {
            string config = "";

            try
            {
                StreamReader reader = new StreamReader(archivo);
                config = reader.ReadToEnd();
                reader.Close();
                return config;
            }
            catch (Exception ex)
            {
                config = "";
            }

            return config;

        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static bool PingHost(string strIP)
        {
            var myPing = new System.Net.NetworkInformation.Ping();
            var reply = myPing.Send(strIP, 1000);

            return reply.Status == System.Net.NetworkInformation.IPStatus.Success;










            //bool pingable = false;
            //System.Net.NetworkInformation.Ping pinger = null;

            //try
            //{
            //    var ip = System.Net.IPAddress.Parse(strIP);

            //    pinger = new System.Net.NetworkInformation.Ping();
            //    var reply = pinger.Send(ip);
            //    pingable = reply.Status == System.Net.NetworkInformation.IPStatus.Success;
            //}
            //catch (System.Net.NetworkInformation.PingException ex)
            //{
            //    // Discard PingExceptions and return false;
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    if (pinger != null)
            //    {
            //        pinger.Dispose();
            //    }
            //}

            //return pingable;
        }
    }

    #region enums

    #region Productos
    public enum OpcionReporteProductos
    {
        [Display(Name = "Historial de Compras")]
        HistorialCompras,

        [Display(Name = "Historial de Compras por Departamentos")]
        HistorialComprasDepartamentos,

        [Display(Name = "Historial de Compras por Dep. Resumen")]
        HistorialComprasDepartamentosResumen,

        [Display(Name = "Listado de Productos Inventario")]
        ListadoProductosInventario,

        [Display(Name = "Historial de Ventas por Departamento")]
        HistorialVentasDepartamento,

        [Display(Name = "Inventario por Departamento")]
        InventarioDepartamento,

        [Display(Name = "Inventario por Departamento Resumen")]
        InventarioDepartamentoResumen,

        [Display(Name = "Historial de Ventas por Departamento Resumen")]
        HistorialVentasDepartamentoResumen,

        [Display(Name = "Historial de Ventas por Clientes")]
        HistorialVentasClientes,

        [Display(Name = "Movimiento de Productos")]
        MovimientoProductos,

        [Display(Name = "Movimiento de Productos Costos")]
        MovimientoProductosCostos,
    }

    public enum TipoProducto
    {
        Servicio,

        [Display(Name = "Producto Terminado")]
        ProductoTerminado,

        [Display(Name = "Producto Combinado")]
        ProductoCombinado,

        [Display(Name = "Servicio Gasto")]
        ServicioGasto,

        [Display(Name = "Producto Gasto")]
        ProductoGasto,

        [Display(Name = "Materia Prima")]
        MateriaPrima
    }

    public enum CriterioBusquedaProductos
    {
        Descripcion,

        CodigoProducto,

        [Display(Name = "Referencia")]
        CodigoSecundario
    }

    public enum TipoServicio
    {
        [Display(Name = "Alquileres, Honorarios y Servicios Profecionales")]
        AlquileresHonorariosServiciosProfecionales,

        [Display(Name = "Contratistas, Ingenieros y Maestros Constructores")]
        ContratistasIngenierosMaestrosConstructores
    }

    public enum TipoMovimientoProducto
    {
        Ajuste,

        Compras,

        [Display(Name = "Dev.Compras")]
        DevCompras,

        [Display(Name = "Dev.Ventas")]
        DevVentas,

        Salidas,

        Transferencia,

        [Display(Name = "Transformación")]
        Transformacion,

        Ventas
    }

    public enum ClasificacionMovimientoProducto
    {
        [Display(Name = "Todos los Movimiento")]
        TodosLosMovimiento,

        [Display(Name = "Movimientos de Salida")]
        MovimientosSalida,

        [Display(Name = "Movimientos de Entrada")]
        MovimientosEntrada
    }

    public enum UsoImpuestoProductos
    {
        [Display(Name = "ITBIS Adelantado")]
        ITBISAdelantado,

        [Display(Name = "ITBIS Sujeto a Proporcionalidad")]
        ITBISSujetoProporcionalidad,

        [Display(Name = "ITBIS Llevado al Costo")]
        ITBISLlevadoAlCosto,

        [Display(Name = "ITBIS Percibido")]
        ITBISPercibido
    }

    public enum TipoMedidaProducto
    {
        Fijas,
        Solicitar
    }

    #endregion

    #region Prendas
    public enum StatusPrenda
    {
        Disponible,

        [Display(Name = "No disponible")]
        NoDisponible,

        Averiado
    }

    public enum CriterioBusquedaPrendas
    {
        [Display(Name = "Código del producto")]
        CodigoProducto,

        Medida,

        [Display(Name = "Ubicación física")]
        UbicacionFisica
    }
    #endregion

    #region Usuarios
    public enum CriterioBusquedaUsuarios
    {
        Nombre,
        Apellido
    }

    public enum ClasificacionUsuario
    {
        A,
        B,
        C
    }
    #endregion

    #region Suplidores
    public enum CriterioBusquedaSuplidores
    {
        Nombre,
        Identificacion,
        Direccion,
        Telefono
    }

    public enum TipoIdentificacion
    {
        [Display(Name = "Cédula")]
        Cedula,
        RNC
    }

    public enum TipoNCF
    {
        [Display(Name = "Proveedor con RNC Registrado")]
        ProveedorRNCRegistrado,

        [Display(Name = "Proveedor Informal")]
        ProveedorInformal,

        [Display(Name = "Gastos Menores")]
        GastosMenores
    }

    public enum TipoProveerdor
    {
        Empresa,

        [Display(Name = "Persona Fisica")]
        PersonaFisica,

        [Display(Name = "Empresa de Seguridad")]
        EmpresaSeguridad
    }

    #endregion

    #region Empleados
    public enum CriterioBusquedaEmpleados
    {
        Nombre,
        Identificacion,
        Direccion,
        Telefono
    }

    public enum EstadoEmpleado
    {
        Soltero,
        Casado,

        [Display(Name = "Unión Libre")]
        UnionLibre
    }

    public enum TipoNomina
    {
        Mensual,
        Quincenal,
        Semanal
    }

    public enum RetenerQuincena
    {
        [Display(Name = "Ambas Quincenas")]
        AmbasQuincenas,

        [Display(Name = "1ra Quincena")]
        PrimeraQuincena,

        [Display(Name = "2da Quincena")]
        SegundaQuincena
    }
    #endregion

    #region Retenciones
    public enum CriterioBusquedaRetenciones
    {
        Nombre
    }
    #endregion

    #region NominasDetalles
    public enum CriterioBusquedaNominasDetalles
    {
        Nombre,
        Concepto
    }

    public enum TipoNominaDetalle
    {
        [Display(Name = "Otros Cargos")]
        OtrosCargos,

        [Display(Name = "Horas Extras Ordinarias")]
        HorasExtrasOrdinarias,

        Incentivos,
        Faltantes,
        Ausencias,
        Regalia,
        Vacaciones,
        Preavisos,
        Cesantia,
        Bonificacion,
        Comisiones,

        [Display(Name = "Incentivos por Objetivos")]
        IncentivosPorObjetivos,

        Percapitas
    }
    #endregion

    #region EntidadesBancarias
    public enum CriterioBusquedaEntidadesBancarias
    {
        Nombre,
        Rnc,
        EntidadBancario
    }
    #endregion

    #region CuentasBancarias
    public enum CriterioBusquedaCuentasBancarias
    {
        Nombre,
        Numero,
        Titular
    }
    #endregion

    #region CuentasBancarias
    public enum MonedaCuentasBancarias
    {
        Pesos,
        Dolar,
        Euro
    }

    public enum FormatoCuentasBancarias
    {
        [Display(Name = "1")]
        uno = 1,

        [Display(Name = "2")]
        dos,

        [Display(Name = "3")]
        tres,

        [Display(Name = "4")]
        cuatro,

        [Display(Name = "5")]
        cinco,

        [Display(Name = "6")]
        seis,

        [Display(Name = "7")]
        siete,

        [Display(Name = "8")]
        ocho,

        [Display(Name = "9")]
        nueve,

        [Display(Name = "10")]
        diez
    }

    public enum TipoCuentasBancarias
    {
        Corriente,
        Ahorro,
        TCredito
    }
    #endregion

    #region ComprobantesFinancieros
    public enum CriterioBusquedaComprobantesFinancieros
    {
        [Display(Name = "Entidad")]
        Nombre,
        NCF
    }

    public enum TipoTransaccionConsulta
    {
        [Display(Name = "0-Depósito")]
        Deposito,

        [Display(Name = "1-Cheque")]
        Cheque,

        [Display(Name = "2-Transferencias")]
        Transferencias,

        [Display(Name = "3-Comisión Bancaria")]
        ComisionBancaria,

        [Display(Name = "4-Cheque Devuelto")]
        ChequeDevuelto,

        [Display(Name = "5-Notas de Crédito")]
        NotasCrédito,

        [Display(Name = "6-Prestamo Bancario")]
        PrestamoBancario,

        [Display(Name = "7-Intereses Ganados")]
        InteresesGanados,

        [Display(Name = "8-Notas de Débito")]
        NotasDebito,
    }
    #endregion

    #region CajasChicas
    public enum CriterioBusquedaFondoCajaChica
    {
        Nombre
    }

    public enum CriterioBusquedaClasificacionCajaChica
    {
        Nombre,
        Cuenta
    }

    public enum Clasificacion606
    {
        [Display(Name = "01-GASTOS DE PERSONAL")]
        Personal,

        [Display(Name = "02-GASTOS POR TRABAJOS, SUMINISTROS Y SERVICIOS")]
        TrabajosSuministrosServicios,

        [Display(Name = "03-ARRENDAMIENTOS")]
        Arrendamientos,

        [Display(Name = "04-GASTOS DE ACTIVOS FIJO")]
        ActivosFijo,

        [Display(Name = "05-GASTOS DE REPRESENTACIÓN")]
        Representación,

        [Display(Name = "06-OTRAS DEDUCCIONES ADMITIDAS")]
        OtrasDeduccionesAdmitidas,

        [Display(Name = "07-GASTOS FINANCIEROS")]
        Financieros,

        [Display(Name = "08-GASTOS EXTRAORDINARIOS")]
        Extraordinarios,

        [Display(Name = "09-COMPRAS Y GASTOS QUE FORMARAN PARTE DEL COSTO DE VENTA")]
        ComprasYGastosDelCostoDeVenta,

        [Display(Name = "10-ADQUISICIONES DE ACTIVOS")]
        AdquisicionesDeActivos,

        [Display(Name = "11-GASTOS DE SEGUROS")]
        Seguros,
    }

    public enum CriterioBusquedaCuentaContable
    {
        Nombre,

        [Display(Name = "Id")]
        IdCuenta,

        [Display(Name = "Número")]
        Numero
    }

    public enum TipoTransaccionCajaChica
    {
        [Display(Name = "0-Retiro")]
        Retiro,

        [Display(Name = "1-Apertura o Aumento")]
        AperturaOAumento,

        [Display(Name = "2-Disminución de Fondo")]
        DisminucionFondo,

        [Display(Name = "3-Reposición")]
        Reposicion
    }

    public enum Status
    {
        Pendientes,
        Pagos,
        Todos
    }
    #endregion

    #region CuentasContables
    public enum CriterioBusquedaCuentasContables
    {
        Nombre,
        Numero
    }

    public enum SubClasificacionCuentasContables
    {
        Ninguno,

        [Display(Name = "Activos Corrientes")]
        ActivosCorrientes,

        [Display(Name = "Activos Fijos")]
        ActivosFijos,

        [Display(Name = "Cargos Diferidos")]
        CargosDiferidos,

        [Display(Name = "Pasivos Corrientes")]
        PasivosCorrientes,

        [Display(Name = "Capital Contable")]
        CapitalContable
    }

    public enum ClasificacionCuentasContables
    {
        Activos,
        Pasivos,
        Capital,
        Ingresos,
        Costos,
        Gastos
    }

    public enum OrigenCuentasContables
    {
        Deudor,
        Acredor
    }

    public enum ResultadosCuentasContables
    {
        Ninguno,
        Ingresos,
        Costos,
        Gastos
    }

    public enum FlujoEfectivoCuentasContables
    {
        Ninguno,
        Operativo,
        Inversiones,
        Financiero,
        Efectivo
    }
    #endregion

    #region EntradasDeDiarios
    public enum CriterioBusquedaEntradasDeDiarios
    {
        Concepto
    }

    public enum TipoEstadoEntradaDeDiarios
    {
        [Display(Name = "Estado Detallado")]
        EstadoDetallado,

        [Display(Name = "Estado Resumen")]
        EstadoResumen
    }
    #endregion

    #region Cobros
    public enum CriterioBusquedaClientes
    {
        Nombre,

        [Display(Name = "Id")]
        IDCliente,

        [Display(Name = "Teléfono")]
        Telefono
    }

    public enum FormaPagoImprimir
    {
        Efectivo,
        Tarjeta,
        Cheque
    }

    public enum TipoCobroCobros
    {
        [Display(Name = "Cobro de Facturas")]
        CobroFacturas,

        [Display(Name = "Cobro por Adelantado")]
        CobroAdelantado
    }

    public enum FormaCobroCobros
    {
        Efectivo,
        Tarjeta,
        Cheque
    }

    #endregion

    #region GlabsHelp
    #region Equipos
    public enum CriterioBusquedaEquipos
    {
        Descripcion,
        Tipo,
    }
    #endregion

    #region EmpresaClientes
    public enum CriterioBusquedaEmpresa
    {
        Nombre,
        Alias
    }
    #endregion

    #region Cliente
    public enum CriterioBusquedaCliente
    {
        Nombre,
        Identificacion
    }
    #endregion
    #endregion

    #region Alquileres
    public enum CondicionVenta
    {
        [Display(Name = "Venta Contado")]
        VentaContado,

        [Display(Name = "Venta Crédito")]
        VentaCredito,

        [Display(Name = "Venta Financiamiento")]
        VentaFinanciamiento,

        [Display(Name = "Venta Contra Entrega")]
        VentaContraEntrega
    }

    public enum PlanCredito
    {
        [Display(Name = "Venta Contado")]
        VentaContado,

        [Display(Name = "Venta Crédito")]
        VentaCredito
    }

    public enum NivelPrecio
    {
        [Display(Name = "Precio Lista")]
        PrecioLista,
        XMayor1,
        XMayor2,
        XMayor3,
        Minimo
    }
    #endregion

    #region Otros
    public enum TipoValor
    {
        String,
        Numeric,
        Date,
        Bool
    };

    public enum Sexo
    {
        [Display(Name = "Masculino")]
        m,

        [Display(Name = "Femenino")]
        f
    }

    public enum EstadoInactividad
    {
        Inactivos,

        Activos,

        Todos,
    }
    #endregion

    #endregion

    #region Viewmodels
    //public class ModalCuentasContablesViewmodel
    //{
    //    #region Propiedades
    //    public List<CuentaContable> CuentasContables { get; set; }
    //    public string StrBusquedaCuentas { get; set; } = string.Empty;
    //    public CriterioBusquedaCuentaContable FiltroCriterioBusquedaCuentas { get; set; }
    //    public bool FiltroDetalle { get; set; }
    //    #endregion

    //    #region Metodos
    //    /// <summary>
    //    /// Llena la propiedad CuentasContables en base a las demas propiedades del viewmodel, utilizando consultas con parametros.
    //    /// </summary>
    //    public void GetCuentasContables()
    //    {
    //        string filtroDetalle = FiltroDetalle ? " detalle = 1 " : " 1=1 ";


    //        int top = (!string.IsNullOrEmpty(StrBusquedaCuentas)) ? 200 : 50;

    //        SqlDataAdapter adapter = new SqlDataAdapter(string.Format("SELECT DISTINCT TOP {0} [IdCuenta] ,[Numero] ,[Nombre] ,[Detalle] ,[Nivel] ,[CuentaControl] ,[Clasificacion] ,[Grupofe] ,[Nominal] ,[EstadoResultado] ,[origen] ,[orden] ,[subgrupo] FROM [Catalogo] WHERE {1} LIKE @StrBusquedaCuentas AND {2} ORDER BY Numero, Nombre;", top, FiltroCriterioBusquedaCuentas.ToString(), filtroDetalle), new SqlConnection());
    //        adapter.SelectCommand.Parameters.AddWithValue("@StrBusquedaCuentas", "%" + StrBusquedaCuentas + "%");

    //        DataTable dtCuentasContables = DataAcces.GetValuesInDataTable(adapter);

    //        CuentasContables = (from row in dtCuentasContables.AsEnumerable()
    //                            select CuentaContable.ConvertRowToCuentaContable(row)).ToList();
    //    }
    //    #endregion
    //}

    public class ModalLogViewmodel
    {
        #region Constructores
        public ModalLogViewmodel()
        {
            ModalTitle = "Lista de mensajes";
            ModalId = "ModalLog";
            MsgList = new List<string>();
            BodyTitle = "Mensajes: ";
        }
        #endregion

        #region Propiedades
        public string ModalTitle { get; set; }
        public string ModalId { get; set; }
        public string BodyTitle { get; set; }
        public List<string> MsgList { get; set; }
        #endregion

        #region Metodos

        #endregion
    }

    public class PagerViewmodel
    {
        #region Constructores
        public PagerViewmodel()
        {
            pagerLenght = 1;
            currentPage = 1;
            itemsForPage = 1;
            passedItems = 0;
            totalItemCount = 1;
            pagesForViews = 5;
        }

        /// <summary>
        /// Inicializa un objeto Pager con los limites preestablecidos.
        /// </summary>
        /// <param name="currentPage">Numero de pagina actual.</param>
        /// <param name="itemsForPage">Cantidad de items por pagina.</param>
        /// <param name="totalItemCount">Numero total de items.</param>
        public PagerViewmodel(int currentPage, int itemsForPage, int totalItemCount, int pagesForViews)
        {
            this.pagesForViews = pagesForViews;
            this.itemsForPage = itemsForPage < 1 ? 1 : itemsForPage;
            this.totalItemCount = totalItemCount;
            pagerLenght = Convert.ToInt32(Math.Ceiling((double)this.totalItemCount / this.itemsForPage));

            if (currentPage < 1)
            {
                this.currentPage = 1;
            }
            else if (currentPage > pagerLenght)
            {
                this.currentPage = pagerLenght;
            }
            else
            {
                this.currentPage = currentPage;
            }

            passedItems = (this.currentPage - 1) * this.itemsForPage;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Cantidad paginas mostradas en el pager.
        /// </summary>
        public int pagerLenght { get; set; }

        /// <summary>
        /// Numero de pagina actual.
        /// </summary>
        public int currentPage { get; set; }

        /// <summary>
        /// Cantidad de items por pagina.
        /// </summary>
        public int itemsForPage { get; set; }

        /// <summary>
        /// Cantidad de rows que ya pasaron. Osea el numero de row desde donde se debe de tomar el siguiente lote  de rows.
        /// </summary>
        public int passedItems { get; set; }

        /// <summary>
        /// Numero total de items.
        /// </summary>
        public int totalItemCount { get; set; }

        /// <summary>
        /// Cantidad deseada de paginas mostradas en el pager a la vez.
        /// </summary>
        public int pagesForViews { get; set; }
        #endregion

        #region Metodos
        //public DataTable GetRows(SqlDataAdapter adapter)
        //{
        //    string query = adapter.SelectCommand.CommandText.ToLower();
        //    int selectIndex = query.IndexOf("select");
        //    var col = 
        //}
        #endregion
    }
    #endregion

    #region Extensiones
    public static partial class Extensiones
    {
        public static WebImage ResizeImage(this HttpPostedFileBase file, int maxWidth = 1366, int maxHeight = 1366)
        {
            return ResizeImage(new WebImage(file.InputStream), maxWidth, maxHeight);
        }

        public static WebImage ResizeImage(this WebImage image, int maxWidth = 1366, int maxHeight = 1366)
        {
            if (image.Height <= maxHeight && image.Width <= maxWidth)
                return image;

            float aspectRatio = Convert.ToSingle(image.Width) / Convert.ToSingle(image.Height);
            int newWidth, newHeight;

            if (image.Width > maxWidth)
            {
                newWidth = maxWidth;
                newHeight = Convert.ToInt32(newWidth / aspectRatio);

                image = image.Resize(newWidth, newHeight);
            }

            if (image.Height > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = Convert.ToInt32(newHeight * aspectRatio);

                image = image.Resize(newWidth, newHeight);
            }

            return image;
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
    #endregion
}
