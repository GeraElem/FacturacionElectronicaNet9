using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;
using FacturaElectronicaProd.ServiceReference2;
using Microsoft.Extensions.Configuration;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class FECompConsultar
    {
        private readonly IConfiguration _configuration;
        private readonly string ambiente;
        private readonly string strUrlWsfev1;

        public FECompConsultar(IConfiguration configuration)
        {
            _configuration = configuration;

            ambiente = _configuration.GetValue<string>("AFIP");

            strUrlWsfev1 = ambiente == "P"
                ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL"
                : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL";
        }

        //string strUrlWsfev1 = (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL" : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL"); //produccion
        ServiceSoapClient servicioCompCons;        //objeto para solicitar el servicio wsfe
        TicketAcceso acceso = new TicketAcceso();
        //objetos solicitados por el método FECompConsultar
        FEAuthRequest auth;
        ServiceReference2.FECompConsultarResponse response;

        //objetos de la DLL
        FECompConsultarResult respuesta;
        private FECompConsultarResult comprobante;


        public FECompConsultarResult ConsultarComprobante(FeCompConsReq cabReq, string pathXml = null, string pathCertificado = null)
        {
            //solicito objeto de autenticación
            auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

            //completo el objeto de request
            var peticion = new FECompConsultaReq
            {
                CbteNro = cabReq.CbteNro,
                CbteTipo = cabReq.CbteTipo,
                PtoVta = cabReq.PtoVta
            };
            
            //ejecuto el método compUltimoAutorizado del servicio
            try
            {
                //servicioCompCons = new ServiceSoapClient(
                //    ServiceSoapClient.EndpointConfiguration.WSFE_HOMO);
                ////guardo el contenido de la respuesta
                //var resultado = servicioCompCons.FECompConsultarAsync(auth, peticion).Result;
                //response = resultado;

                var endpoint = new EndpointAddress(strUrlWsfev1);

                servicioCompCons = new ServiceSoapClient(
                    ServiceSoapClient.EndpointConfiguration.ServiceSoap,
                    endpoint
                );

                var resultado = servicioCompCons
                    .FECompConsultarAsync(auth, peticion)
                    .GetAwaiter()
                    .GetResult();

                response = resultado;
            }
            catch (Exception excepcionAlInvocarWsaa)
            {
                throw new Exception("***Error INVOCANDO al servicio WSFE : " + excepcionAlInvocarWsaa.Message);
            }

            //creo el objeto a devolver
            respuesta = new FECompConsultarResult();

            respuesta.comprobante = new ClasesResponse.FECompConsultarResponse
            {
                concepto = response.Body.FECompConsultarResult.ResultGet.Concepto,
                docTipo = response.Body.FECompConsultarResult.ResultGet.DocTipo,
                docNro = response.Body.FECompConsultarResult.ResultGet.DocNro,
                cbteDesde = response.Body.FECompConsultarResult.ResultGet.CbteDesde,
                cbteHasta = response.Body.FECompConsultarResult.ResultGet.CbteHasta,
                cbteFch = response.Body.FECompConsultarResult.ResultGet.CbteFch,
                impTotal = response.Body.FECompConsultarResult.ResultGet.ImpTotal,
                impTotConc = response.Body.FECompConsultarResult.ResultGet.ImpTotConc,
                impNeto = response.Body.FECompConsultarResult.ResultGet.ImpNeto,
                impOpEx = response.Body.FECompConsultarResult.ResultGet.ImpOpEx,
                impTrib = response.Body.FECompConsultarResult.ResultGet.ImpTrib,
                impIVA = response.Body.FECompConsultarResult.ResultGet.ImpIVA,
                monId = response.Body.FECompConsultarResult.ResultGet.MonId,
                monCotiz = response.Body.FECompConsultarResult.ResultGet.MonCotiz,
                resultado = response.Body.FECompConsultarResult.ResultGet.Resultado,
                cae = response.Body.FECompConsultarResult.ResultGet.CodAutorizacion,
                fechaProceso = response.Body.FECompConsultarResult.ResultGet.FchProceso,
                ptoVta = response.Body.FECompConsultarResult.ResultGet.PtoVta,
                cbteTipo = response.Body.FECompConsultarResult.ResultGet.CbteTipo,
                fchVtoPago = response.Body.FECompConsultarResult.ResultGet.FchVtoPago,
                fchVto = response.Body.FECompConsultarResult.ResultGet.FchVto
            };

            //respuesta errores si no es nulo
            if (response.Body.FECompConsultarResult.Errors != null)
            {
                respuesta.errores = new ErroresResponse[response.Body.FECompConsultarResult.Errors.Length];
                for (int i = 0; i < response.Body.FECompConsultarResult.Errors.Length; i++)
                {
                    respuesta.errores[i] = new ErroresResponse
                    {
                        code = response.Body.FECompConsultarResult.Errors[i].Code,
                        msg = response.Body.FECompConsultarResult.Errors[i].Msg
                    };
                }
            }

            //respuesta eventos si no es nulo
            if (response.Body.FECompConsultarResult.Events != null)
            {
                respuesta.eventos = new EventosResponse[response.Body.FECompConsultarResult.Events.Length];
                for (int i = 0; i < response.Body.FECompConsultarResult.Events.Length; i++)
                {
                    respuesta.eventos[i] = new EventosResponse
                    {
                        code = response.Body.FECompConsultarResult.Events[i].Code,
                        msg = response.Body.FECompConsultarResult.Events[i].Msg
                    };
                }
            }

            return respuesta;
        }
    }
}
