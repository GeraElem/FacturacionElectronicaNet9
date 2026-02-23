using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class FEParamGetTiposDoc
    {
        string strUrlWsfev1 = (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL" : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL"); //produccion
        WSFEV1.Service servicioCompAut; //objeto para solicitar el servicio wsfe
        TicketAcceso acceso = new TicketAcceso();
        //objetos solicitados por el método FEParamGetTiposOpcional
        WSFEV1.FEAuthRequest auth;
        WSFEV1.DocTipoResponse response;
        DocTipoResponse respuesta;

        public DocTipoResponse obenerDocumentos(string pathXml = null, string pathCertificado = null)
        {
            try
            {
                //solicito objeto de autenticación
                auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

                //ejecuto el método compUltimoAutorizado del servicio
                try
                {
                    servicioCompAut = new WSFEV1.Service();
                    servicioCompAut.Url = strUrlWsfev1;
                    //guardo el contenido de la respuesta
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    response = servicioCompAut.FEParamGetTiposDoc(auth);
                }
                catch (Exception excepcionAlInvocarWsaa)
                {
                    throw new Exception("***Error INVOCANDO al servicio FEParamGetTiposDoc : " + excepcionAlInvocarWsaa.Message);
                }

                //creo el objeto a devolver
                respuesta = new DocTipoResponse();

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

                //opciones
                if (response.ResultGet != null)
                {
                    respuesta.documentos = new DocumentoRequest[response.ResultGet.Length];
                    for (int i = 0; i < response.ResultGet.Length; i++)
                    {
                        respuesta.documentos[i] = new DocumentoRequest();
                        respuesta.documentos[i].id = response.ResultGet[i].Id;
                        respuesta.documentos[i].valor = response.ResultGet[i].Desc;
                    }
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
