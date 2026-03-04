using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsumirWSFactElect.Seguridad
{
    internal class LoginTicket
    {
        public string Service { get; private set; }
        public XmlDocument XmlLoginTicketRequest { get; private set; }
        public XmlDocument XmlLoginTicketResponse { get; private set; }

        private static int _globalUniqueID = 0;

        private const string LoginTicketTemplate =
            "<loginTicketRequest>" +
            "<header>" +
            "<uniqueId></uniqueId>" +
            "<generationTime></generationTime>" +
            "<expirationTime></expirationTime>" +
            "</header>" +
            "<service></service>" +
            "</loginTicketRequest>";

        public XmlDocument ObtenerLoginTicketResponse(
            string servicio,
            string urlWsaa,
            string rutaCertificadoP12,
            string passwordCertificado,
            string proxy = null,
            string proxyUser = null,
            string proxyPassword = null)
        {
            const string ID_FNC = "[ObtenerLoginTicketResponse]";

            try
            {
                Service = servicio;

                // ======================================================
                // PASO 1 – Crear LoginTicketRequest
                // ======================================================
                XmlLoginTicketRequest = new XmlDocument();
                XmlLoginTicketRequest.LoadXml(LoginTicketTemplate);

                int uniqueId = Interlocked.Increment(ref _globalUniqueID);

                XmlLoginTicketRequest.SelectSingleNode("//uniqueId")!.InnerText = uniqueId.ToString();
                XmlLoginTicketRequest.SelectSingleNode("//generationTime")!.InnerText =
                    DateTime.UtcNow.AddMinutes(-10).ToString("s");
                XmlLoginTicketRequest.SelectSingleNode("//expirationTime")!.InnerText =
                    DateTime.UtcNow.AddMinutes(10).ToString("s");
                XmlLoginTicketRequest.SelectSingleNode("//service")!.InnerText = servicio;

                // ======================================================
                // PASO 2 – Firmar LoginTicketRequest
                // ======================================================
                var certificado = CertificadosX509Lib.ObtenerCertificadoDesdeArchivo(
                    rutaCertificadoP12,
                    passwordCertificado
                );

                byte[] requestBytes = Encoding.UTF8.GetBytes(XmlLoginTicketRequest.OuterXml);
                byte[] signedBytes = CertificadosX509Lib.FirmaBytesMensaje(requestBytes, certificado);
                string cmsFirmadoBase64 = Convert.ToBase64String(signedBytes);

                // ======================================================
                // PASO 3 – Invocar WSAA
                // ======================================================
                var servicioWsaa = new WSAA.LoginCMSService
                {
                    Url = urlWsaa
                };

                if (!string.IsNullOrEmpty(proxy))
                {
                    servicioWsaa.Proxy = new WebProxy(proxy, true);

                    if (!string.IsNullOrEmpty(proxyUser))
                    {
                        servicioWsaa.Proxy.Credentials =
                            new NetworkCredential(proxyUser, proxyPassword);
                    }
                }

                string loginTicketResponse = servicioWsaa.loginCms(cmsFirmadoBase64);

                // ======================================================
                // PASO 4 – Parsear respuesta
                // ======================================================
                XmlLoginTicketResponse = new XmlDocument();
                XmlLoginTicketResponse.LoadXml(loginTicketResponse);

                return XmlLoginTicketResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{ID_FNC} Error obteniendo LoginTicketResponse: {ex.Message}",
                    ex
                );
            }
        }
    }
}