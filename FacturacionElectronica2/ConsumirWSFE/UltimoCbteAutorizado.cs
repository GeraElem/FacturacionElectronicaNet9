using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacturaElectronica.ConsumirWSFE.ClasesResponse;

namespace FacturaElectronica.ConsumirWSFE
{
    public class UltimoCbteAutorizado
    {
        const string strUrlWsfev1 = (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL" : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL"); //produccion
        //const string strUrlWsfev1 = "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL"; //produccion
        WSFEV1.Service servicioCompAut; //objeto para solicitar el servicio wsfe
        TicketAcceso acceso = new TicketAcceso();
        //objetos solicitados por el método FECAEcompUltimoAutorizado
        WSFEV1.FEAuthRequest auth;
        WSFEV1.FERecuperaLastCbteResponse response;
        CbteAutResponse respuesta;

        public CbteAutResponse obtenerUltimoCbtAut(int ptoVta, int cbteTipo)
        {
            //solicito objeto de autenticación
            auth = acceso.ObtenerCredencialesTA();

            //ejecuto el método compUltimoAutorizado del servicio
            try
            {
                servicioCompAut = new WSFEV1.Service();
                servicioCompAut.Url = strUrlWsfev1;
                //guardo el contenido de la respuesta
                response = servicioCompAut.FECompUltimoAutorizado(auth, ptoVta, cbteTipo);
            }
            catch (Exception excepcionAlInvocarWsaa)
            {
                throw new Exception("***Error INVOCANDO al servicio WSFE : " + excepcionAlInvocarWsaa.Message);
            }

            //creo el objeto a devolver
            respuesta = new CbteAutResponse();

            //respuesta errores si no es nulo
            if (response.Errors != null)
            {
                respuesta.errores = new ErroresResponse[response.Errors.Length];
                for (int i = 0; i < response.Errors.Length; i++)
                {
                    respuesta.errores[i] = new ErroresResponse();
                    respuesta.errores[i].code = response.Errors[i].Code;
                    respuesta.errores[i].msg = response.Errors[i].Msg;
                }
            }

            //respuesta eventos si no es nulo
            if (response.Events != null)
            {
                respuesta.eventos = new EventosResponse[response.Events.Length];
                for (int i = 0; i < response.Events.Length; i++)
                {
                    respuesta.eventos[i] = new EventosResponse();
                    respuesta.eventos[i].code = response.Events[i].Code;
                    respuesta.eventos[i].msg = response.Events[i].Msg;
                }
            }

            respuesta.cbteNro = response.CbteNro;
            respuesta.cbteTipo = response.CbteTipo;
            respuesta.ptoVta = response.PtoVta;

            return respuesta;
        }
    }
}
