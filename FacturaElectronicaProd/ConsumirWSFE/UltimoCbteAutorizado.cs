using System;
using System.Net;
using System.ServiceModel;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;
using FacturaElectronicaProd.ServiceReference2;
using Microsoft.Extensions.Configuration;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class UltimoCbteAutorizado
    {
        private readonly IConfiguration _configuration;
        private readonly string ambiente;
        private readonly string strUrlWsfev1;

        private readonly TicketAcceso acceso;
        private ServiceSoapClient servicio;

        public UltimoCbteAutorizado(IConfiguration configuration)
        {
            _configuration = configuration;

            ambiente = _configuration.GetValue<string>("AFIP");

            strUrlWsfev1 = ambiente == "P"
                ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx"
                : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx";

            acceso = new TicketAcceso(configuration);
        }

        public CbteAutResponse ObtenerUltimoCbtAut(
            int ptoVta,
            int cbteTipo,
            string pathXml = null,
            string pathCertificado = null)
        {
            // 🔐 Autenticación
            var auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // 🧩 Cliente WCF
            var endpoint = new EndpointAddress(strUrlWsfev1);

            servicio = new ServiceSoapClient(
                ServiceSoapClient.EndpointConfiguration.ServiceSoap,
                endpoint
            );

            // 📡 Llamada al WS
            var response = servicio
                .FECompUltimoAutorizadoAsync(auth, ptoVta, cbteTipo)
                .GetAwaiter()
                .GetResult();

            var result = response.Body.FECompUltimoAutorizadoResult;

            // 📦 Mapeo DTO propio
            var respuesta = new CbteAutResponse
            {
                cbteNro = result.CbteNro,
                cbteTipo = result.CbteTipo,
                ptoVta = result.PtoVta
            };

            if (result.Errors != null)
            {
                respuesta.errores = Array.ConvertAll(
                    result.Errors,
                    e => new ErroresResponse
                    {
                        code = e.Code,
                        msg = e.Msg
                    });
            }

            if (result.Events != null)
            {
                respuesta.eventos = Array.ConvertAll(
                    result.Events,
                    e => new EventosResponse
                    {
                        code = e.Code,
                        msg = e.Msg
                    });
            }

            return respuesta;
        }
    }
}