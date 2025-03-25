using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;//conectarnos a web config
using System.Data;//recibir y enviar objetos contenedores de datos
using MySql.Data.MySqlClient;

namespace cine2doseg.Models
{
    public class clsSucursal
    {
        // Propiedades de la clase
        public string cve { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string url { get; set; }
        public string logo { get; set; }

        // Cadena de conexión desde web.config
        private string cadConn = ConfigurationManager.ConnectionStrings["bd_cine"].ConnectionString;

        // Constructores
        public clsSucursal() { }

        public clsSucursal(string cve, string nombre, string direccion, string url, string logo)
        {
            this.cve = cve;
            this.nombre = nombre;
            this.direccion = direccion;
            this.url = url;
            this.logo = logo;
        }

        // Método para insertar sucursal
        public DataSet spInsSucursales()
        {
            DataSet ds = new DataSet();

            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                using (MySqlCommand cmd = new MySqlCommand("spInsSucursales", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("p_nombre", this.nombre);
                    cmd.Parameters.AddWithValue("p_direccion", this.direccion);
                    cmd.Parameters.AddWithValue("p_homeweb", this.url);
                    cmd.Parameters.AddWithValue("p_logo", this.logo);

                    // Parámetro de salida
                    MySqlParameter outParam = new MySqlParameter("p_resultado", MySqlDbType.Int32);
                    outParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outParam);

                    cnn.Open();

                    // Ejecutar procedimiento
                    cmd.ExecuteNonQuery();

                    // Crear tabla con el resultado
                    DataTable dt = new DataTable("Resultado");
                    dt.Columns.Add("resultado", typeof(int));
                    dt.Rows.Add(outParam.Value);
                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }

        // Método para consultar todas las sucursales
        public DataSet vwRptSucursales()
        {
            DataSet ds = new DataSet();

            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                string query = "SELECT * FROM vwRptSucursales";
                MySqlDataAdapter da = new MySqlDataAdapter(query, cnn);
                da.Fill(ds, "vwRptSucursales");
            }

            return ds;
        }

        // Método para validar sucursal (nuevo)
        public DataSet spValidarSucursal()
        {
            DataSet ds = new DataSet();

            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                using (MySqlCommand cmd = new MySqlCommand("spValidarSucursal", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("p_nombre", this.nombre);
                    cmd.Parameters.AddWithValue("p_url", this.url);

                    // Parámetro de salida
                    MySqlParameter outParam = new MySqlParameter("p_resultado", MySqlDbType.Int32);
                    outParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outParam);

                    cnn.Open();

                    // Ejecutar procedimiento
                    cmd.ExecuteNonQuery();

                    // Crear tabla con el resultado
                    DataTable dt = new DataTable("Resultado");
                    dt.Columns.Add("resultado", typeof(int));
                    dt.Rows.Add(outParam.Value);
                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }
    }

}