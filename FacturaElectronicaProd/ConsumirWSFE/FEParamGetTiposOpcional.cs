using System;
using System.Linq;
using System.Net;
using System.ServiceModel;
using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;
using FacturaElectronicaProd.ServiceReference2;
using Microsoft.Extensions.Configuration;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class FEParamGetTiposOpcional
    {
        private readonly IConfiguration _configuration;
        private readonly string ambiente;
        private readonly string strUrlWsfev1;

        private ServiceSoapClient servicioCompAut;
        private readonly TicketAcceso acceso;

        // AFIP
        private FEAuthRequest auth;
        private FEParamGetTiposOpcionalResponse response;

        // DTO propio
        private ClasesResponse.OpcionalTipoResponse respuesta;

        public FEParamGetTiposOpcional(IConfiguration configuration)
        {
            _configuration = configuration;

            ambiente = _configuration.GetValue<string>("AFIP");

            strUrlWsfev1 = ambiente == "P"
                ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL"
                : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL";

            acceso = new TicketAcceso(configuration);
        }

        public ClasesResponse.OpcionalTipoResponse ObtenerOpcionales(
            string pathXml = null,
            string pathCertificado = null)
        {
            try
            {
                // 1️⃣ Autenticación
                auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

                try
                {
                    // 2️⃣ Cliente WCF con endpoint dinámico
                    var endpoint = new EndpointAddress(strUrlWsfev1);

                    servicioCompAut = new ServiceSoapClient(
                        ServiceSoapClient.EndpointConfiguration.ServiceSoap,
                        endpoint
                    );

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    // 3️⃣ Llamada al WS
                    response = servicioCompAut
                        .FEParamGetTiposOpcionalAsync(auth)
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "***Error INVOCANDO al servicio FEParamGetTiposOpcional : " + ex.Message);
                }

                // 4️⃣ Mapeo a DTO propio
                respuesta = new ClasesResponse.OpcionalTipoResponse();

                var result = response.Body.FEParamGetTiposOpcionalResult;

                // errores
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

                // eventos
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

                // opciones
                if (result.ResultGet != null)
                {
                    respuesta.opciones = result.ResultGet
                        .Select(o => new OpcionalRequest
                        {
                            id = o.Id,
                            valor = o.Desc
                        })
                        .ToArray();
                }

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}