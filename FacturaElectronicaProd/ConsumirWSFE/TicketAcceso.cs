using System;
using System.IO;
using System.Xml;
using FacturaElectronicaProd.Seguridad;
using FacturaElectronicaProd.ServiceReference2;
using Microsoft.Extensions.Configuration;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class TicketAcceso
    {
        private readonly IConfiguration _configuration;

        private readonly string ambiente;
        private readonly string urlWsaa;
        private readonly string rutaCertificado;
        private readonly string archivoXmlWsfe;
        private readonly string archivoXmlPadron;

        private XmlDocument xmlTicket;
        private LoginTicket ticket;

        public TicketAcceso(IConfiguration configuration)
        {
            _configuration = configuration;

            ambiente = _configuration.GetValue<string>("AFIP");

            urlWsaa = ambiente == "P"
                ? "https://wsaa.afip.gov.ar/ws/services/LoginCms?WSDL"
                : "https://wsaahomo.afip.gov.ar/ws/services/LoginCms?WSDL";

            var basePath = AppContext.BaseDirectory + "FE\\";

            rutaCertificado = basePath +
                (ambiente == "P" ? "ClavePrivadaProd.p12" : "ClavePrivada.p12");

            archivoXmlWsfe = basePath +
                (ambiente == "P" ? "dataProd.xml" : "data.xml");

            archivoXmlPadron = basePath +
                (ambiente == "P" ? "dataPadronProd.xml" : "dataPadron.xml");
        }

        // ===============================
        // WSFE
        // ===============================
        public FEAuthRequest ObtenerCredencialesTA(
            string pathXml = null,
            string pathCertificado = null)
        {
            return ObtenerTA(
                servicio: "wsfe",
                archivo: pathXml ?? archivoXmlWsfe,
                certificado: pathCertificado ?? rutaCertificado);
        }

        // ===============================
        // PADRON
        // ===============================
        public FEAuthRequest PadronObtenerCredencialesTA(string servicio)
        {
            return ObtenerTA(
                servicio: servicio,
                archivo: archivoXmlPadron,
                certificado: rutaCertificado);
        }

        // ===============================
        // MÉTODO CENTRAL
        // ===============================
        private FEAuthRequest ObtenerTA(
            string servicio,
            string archivo,
            string certificado)
        {
            xmlTicket = new XmlDocument();
            var ahora = DateTime.Now;

            if (!File.Exists(archivo))
            {
                GenerarTA(servicio, archivo, certificado);
            }

            xmlTicket.Load(archivo);

            var expiration =
                DateTime.Parse(xmlTicket.SelectSingleNode("//expirationTime").InnerText);

            if (expiration <= ahora)
            {
                GenerarTA(servicio, archivo, certificado);
                xmlTicket.Load(archivo);
            }

            var auth = new FEAuthRequest
            {
                Token = xmlTicket.SelectSingleNode("//token").InnerText,
                Sign = xmlTicket.SelectSingleNode("//sign").InnerText
            };

            var destination =
                xmlTicket.SelectSingleNode("//destination").InnerText;

            auth.Cuit = long.Parse(destination.Substring(18, 11));

            return auth;
        }

        private void GenerarTA(
            string servicio,
            string archivo,
            string certificado)
        {
            ticket = new LoginTicket();

            xmlTicket = ticket.ObtenerLoginTicketResponse(
                servicio,
                urlWsaa,
                certificado,
                null,
                null,
                null);

            xmlTicket.Save(archivo);
        }
    }
}