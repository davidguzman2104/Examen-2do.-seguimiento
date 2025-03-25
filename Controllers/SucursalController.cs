using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//----------------------
using cine2doseg.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace cine2doseg.Controllers
{
    public class SucursalController : ApiController
    {
        // Endpoint para obtener reporte de sucursales
        [HttpGet]
        [Route("cine/sucursal/vwrptsucursales")]
        public clsApiStatus vwRptSucursales()
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                // Crear instancia del modelo sin parámetros
                clsSucursal objSucursal = new clsSucursal();

                // Obtener datos de sucursales
                ds = objSucursal.vwRptSucursales();

                // Configurar objeto de respuesta
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Reporte de sucursales obtenido correctamente";

                // Convertir DataTable a JSON
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"sucursales\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al obtener sucursales";
                jsonResp.Add("error", ex.Message);
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

        // Endpoint para insertar nueva sucursal
        [HttpPost]
        [Route("cine/sucursal/spinssucursales")]
        public clsApiStatus spInsSucursales([FromBody] clsSucursal modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                // Validar modelo recibido
                if (modelo == null)
                {
                    throw new ArgumentNullException("El modelo de sucursal no puede ser nulo");
                }

                // Crear instancia del modelo con parámetros
                clsSucursal objSucursal = new clsSucursal(
                    null, // cve se genera automáticamente
                    modelo.nombre,
                    modelo.direccion,
                    modelo.url,
                    modelo.logo
                );

                // Intentar insertar la sucursal
                ds = objSucursal.spInsSucursales();

                // Obtener resultado de la operación
                int resultado = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                // Configurar respuesta según resultado
                objRespuesta.statusExec = true;
                objRespuesta.ban = resultado;

                if (resultado == 0)
                {
                    objRespuesta.msg = "Sucursal registrada exitosamente";
                    jsonResp.Add("mensaje", "Registro exitoso");
                }
                else if (resultado == 1)
                {
                    objRespuesta.msg = "El nombre de sucursal ya existe";
                    jsonResp.Add("error", "Nombre duplicado");
                }
                else if (resultado == 2)
                {
                    objRespuesta.msg = "La URL web ya está registrada";
                    jsonResp.Add("error", "URL duplicada");
                }
                else
                {
                    objRespuesta.msg = "Resultado desconocido";
                    jsonResp.Add("error", "Código de resultado no reconocido");
                }

                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al procesar la solicitud";
                jsonResp.Add("error", ex.Message);
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }
    }
}