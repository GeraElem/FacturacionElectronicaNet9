using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;
using System.Net;
using System.Globalization;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class FEParamGetCotizacion
    {
        //string strUrlWsfev1 = (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL" : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL"); //produccion
        //WSFEV1.Service servicioCompAut; //objeto para solicitar el servicio wsfe
        //TicketAcceso acceso = new TicketAcceso();
        ////objetos solicitados por el método FEParamGetTiposOpcional
        //WSFEV1.FEAuthRequest auth;
        //WSFEV1.FECotizacionResponse response;
        //FECotizacionResponse respuesta;

        //public FECotizacionResponse obenerCotizacion(string monId, string fechaCotiz,string pathXml = null, string pathCertificado = null)
        //{
        //    try
        //    {
        //        //solicito objeto de autenticación
        //        auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

        //        //ejecuto el método compUltimoAutorizado del servicio
        //        try
        //        {
        //            servicioCompAut = new WSFEV1.Service();
        //            servicioCompAut.Url = strUrlWsfev1;
        //            //guardo el contenido de la respuesta
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //            var _fechaTemporal = DateTime.ParseExact(fechaCotiz, "yyyyMMdd",CultureInfo.InvariantCulture);
        //            do
        //            {
        //                response = servicioCompAut.FEParamGetCotizacion(auth, monId, _fechaTemporal.ToString("yyyyMMdd"));
        //                _fechaTemporal = _fechaTemporal.AddDays(-1);
        //            }
        //            while (response.Errors != null && response.Errors.Any(x => x.Code == 602));

        //        }
        //        catch (Exception excepcionAlInvocarWsaa)
        //        {
        //            throw new Exception("***Error INVOCANDO al servicio FEParamGetCotizacion : " + excepcionAlInvocarWsaa.Message);
        //        }

        //        //creo el objeto a devolver
        //        respuesta = new FECotizacionResponse();

        //        //respuesta errores si no es nulo
        //        if (response.Errors != null)
        //        {
        //            respuesta.errores = new ErroresResponse[response.Errors.Length];
        //            for (int i = 0; i < response.Errors.Length; i++)
        //            {
        //                respuesta.errores[i] = new ErroresResponse();
        //                respuesta.errores[i].code = response.Errors[i].Code;
        //                respuesta.errores[i].msg = response.Errors[i].Msg;
        //            }
        //        }

        //        //respuesta eventos si no es nulo
        //        if (response.Events != null)
        //        {
        //            respuesta.eventos = new EventosResponse[response.Events.Length];
        //            for (int i = 0; i < response.Events.Length; i++)
        //            {
        //                respuesta.eventos[i] = new EventosResponse();
        //                respuesta.eventos[i].code = response.Events[i].Code;
        //                respuesta.eventos[i].msg = response.Events[i].Msg;
        //            }
        //        }

        //        //devolvemos la cotizacion
        //        if (response.ResultGet != null)
        //        {

        //            respuesta.cotizacion = new CotizacionRequest();
        //            respuesta.cotizacion.MonId = response.ResultGet.MonId;
        //            respuesta.cotizacion.MonCotiz = response.ResultGet.MonCotiz;
        //            respuesta.cotizacion.FchCotiz = response.ResultGet.FchCotiz;
        //        }

        //        return respuesta;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
