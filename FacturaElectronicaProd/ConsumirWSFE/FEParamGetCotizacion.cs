using System;
using System.Linq;
using System.Net;
using System.Globalization;
using System.ServiceModel;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;
using FacturaElectronicaProd.ServiceReference2;
using Microsoft.Extensions.Configuration;
using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;
using FECotizacionResponse = FacturaElectronicaProd.ConsumirWSFE.ClasesResponse.FECotizacionResponse;


namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class FEParamGetCotizacion
    {
        private readonly IConfiguration _configuration;
        private readonly string ambiente;
        private readonly string strUrlWsfev1;

        ServiceSoapClient servicioCompAut;
        private readonly TicketAcceso acceso;

        FEAuthRequest auth;
        FEParamGetCotizacionResponse response;
        FECotizacionResponse respuesta;

        public FEParamGetCotizacion(IConfiguration configuration)
        {
            _configuration = configuration;

            ambiente = _configuration.GetValue<string>("AFIP");

            strUrlWsfev1 = ambiente == "P"
                ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL"
                : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL";

            acceso = new TicketAcceso(configuration);

        }

        public FECotizacionResponse ObtenerCotizacion(
            string monId,
            string fechaCotiz,
            string pathXml = null,
            string pathCertificado = null)
        {
            // Autenticación
            auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

            try
            {
                var endpoint = new EndpointAddress(strUrlWsfev1);

                servicioCompAut = new ServiceSoapClient(
                    ServiceSoapClient.EndpointConfiguration.ServiceSoap,
                    endpoint
                );

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var fechaTemporal = DateTime.ParseExact(
                    fechaCotiz,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );

                do
                {
                    response = servicioCompAut
                        .FEParamGetCotizacionAsync(auth, monId, fechaTemporal.ToString("yyyyMMdd"))
                        .GetAwaiter()
                        .GetResult();

                    fechaTemporal = fechaTemporal.AddDays(-1);
                }
                while (
                    response.Body.FEParamGetCotizacionResult.Errors != null &&
                    response.Body.FEParamGetCotizacionResult.Errors.Any(e => e.Code == 602)
                );
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "***Error INVOCANDO FEParamGetCotizacion : " + ex.Message
                );
            }

            // Mapeo de respuesta
            respuesta = new FECotizacionResponse();

            var result = response.Body.FEParamGetCotizacionResult;

            if (result.Errors != null)
            {
                respuesta.errores = result.Errors
                    .Select(e => new ErroresResponse
                    {
                        code = e.Code,
                        msg = e.Msg
                    })
                    .ToArray();
            }

            if (result.Events != null)
            {
                respuesta.eventos = result.Events
                    .Select(e => new EventosResponse
                    {
                        code = e.Code,
                        msg = e.Msg
                    })
                    .ToArray();
            }

            if (result.ResultGet != null)
            {
                respuesta.cotizacion = new CotizacionRequest
                {
                    MonId = result.ResultGet.MonId,
                    MonCotiz = result.ResultGet.MonCotiz,
                    FchCotiz = result.ResultGet.FchCotiz
                };
            }

            return respuesta;
        }
    }
}