using System;
using System.Net;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;
using FacturaElectronicaProd.ServiceReference2;
using Microsoft.Extensions.Configuration;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class UltimoCbteAutorizado
    {
        private readonly string strUrlWsfev1 =
            (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P"
                ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx"
                : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx");

        private ServiceSoapClient servicio;
        private readonly TicketAcceso acceso;

        public UltimoCbteAutorizado(IConfiguration configuration)
        {
            acceso = new TicketAcceso(configuration);
        }

        public CbteAutResponse obtenerUltimoCbtAut(
            int ptoVta,
            int cbteTipo,
            string pathXml = null,
            string pathCertificado = null)
        {
            // 🔐 auth
            FEAuthRequest auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            servicio = new ServiceSoapClient(
                ServiceSoapClient.EndpointConfiguration.ServiceSoap,
                strUrlWsfev1
            );

            var response = servicio
                .FECompUltimoAutorizadoAsync(auth, ptoVta, cbteTipo)
                .GetAwaiter()
                .GetResult();

            var result = response.Body.FECompUltimoAutorizadoResult;

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
                    e => new ErroresResponse { code = e.Code, msg = e.Msg }
                );
            }

            if (result.Events != null)
            {
                respuesta.eventos = Array.ConvertAll(
                    result.Events,
                    e => new EventosResponse { code = e.Code, msg = e.Msg }
                );
            }

            return respuesta;
        }
    }
}