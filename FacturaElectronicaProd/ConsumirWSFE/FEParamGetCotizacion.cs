using System;
using System.Linq;
using System.Net;
using System.Globalization;
using System.ServiceModel;
using Microsoft.Extensions.Configuration;

using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;
using FacturaElectronicaProd.ServiceReference2;

using FECotizacionResponse =
    FacturaElectronicaProd.ConsumirWSFE.ClasesResponse.FECotizacionResponse;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class FEParamGetCotizacion
    {
        private readonly IConfiguration _configuration;
        private readonly string ambiente;
        private readonly string strUrlWsfev1;

        private readonly TicketAcceso acceso;
        private ServiceSoapClient servicioCompAut;

        private FEAuthRequest auth;
        private FEParamGetCotizacionResponse response;
        private FECotizacionResponse respuesta;

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
            // ===============================
            // AUTENTICACIÓN
            // ===============================
            auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var endpoint = new EndpointAddress(strUrlWsfev1);

                servicioCompAut = new ServiceSoapClient(
                    ServiceSoapClient.EndpointConfiguration.ServiceSoap,
                    endpoint
                );

                var fechaTemporal = DateTime.ParseExact(
                    fechaCotiz,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );

                int intentos = 0;
                const int maxIntentos = 10;

                do
                {
                    response = servicioCompAut
                        .FEParamGetCotizacionAsync(
                            auth,
                            monId,
                            fechaTemporal.ToString("yyyyMMdd")
                        )
                        .GetAwaiter()
                        .GetResult();

                    var result = response.Body.FEParamGetCotizacionResult;

                    // ✅ SI HAY RESULTADO, TERMINAMOS
                    if (result.ResultGet != null)
                        break;

                    // ❌ SI NO ES ERROR 602, TERMINAMOS
                    if (result.Errors == null ||
                        !result.Errors.Any(e => e.Code == 602))
                        break;

                    // Retrocedemos un día
                    fechaTemporal = fechaTemporal.AddDays(-1);
                    intentos++;

                }
                while (intentos < maxIntentos);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "***Error INVOCANDO FEParamGetCotizacion : " + ex.Message,
                    ex
                );
            }

            // ===============================
            // MAPEO DE RESPUESTA
            // ===============================
            respuesta = new FECotizacionResponse();

            var resultado = response.Body.FEParamGetCotizacionResult;

            if (resultado.Errors != null)
            {
                respuesta.errores = resultado.Errors
                    .Select(e => new ErroresResponse
                    {
                        code = e.Code,
                        msg = e.Msg
                    })
                    .ToArray();
            }

            if (resultado.Events != null)
            {
                respuesta.eventos = resultado.Events
                    .Select(e => new EventosResponse
                    {
                        code = e.Code,
                        msg = e.Msg
                    })
                    .ToArray();
            }

            if (resultado.ResultGet != null)
            {
                respuesta.cotizacion = new CotizacionRequest
                {
                    MonId = resultado.ResultGet.MonId,
                    MonCotiz = resultado.ResultGet.MonCotiz,
                    FchCotiz = resultado.ResultGet.FchCotiz
                };
            }

            return respuesta;
        }
    }
}