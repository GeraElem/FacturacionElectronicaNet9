using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace FacturaElectronicaProd.Seguridad
{
    internal class LoginTicket
    {
        public string Service { get; private set; }
        public XmlDocument XmlLoginTicketRequest { get; private set; }
        public XmlDocument XmlLoginTicketResponse { get; private set; }

        private static uint _globalUniqueID = 0;

        private const string XmlTemplate =
            "<loginTicketRequest>" +
                "<header>" +
                    "<uniqueId></uniqueId>" +
                    "<generationTime></generationTime>" +
                    "<expirationTime></expirationTime>" +
                "</header>" +
                "<service></service>" +
            "</loginTicketRequest>";

        /// <summary>
        /// Obtiene el LoginTicketResponse desde WSAA
        /// </summary>
        public XmlDocument ObtenerLoginTicketResponse(
            string servicio,
            string urlWsaa,
            string rutaCertificadoP12,
            string passwordCertificado = null,
            object value = null,
            object value1 = null)
        {
            const string ID_FNC = "[ObtenerLoginTicketResponse]";

            string cmsFirmadoBase64;
            string loginTicketResponseXml;

            try
            {
                // ======================================================
                // PASO 1: CREAR LOGIN TICKET REQUEST
                // ======================================================
                _globalUniqueID++;

                XmlLoginTicketRequest = new XmlDocument();
                XmlLoginTicketRequest.LoadXml(XmlTemplate);

                XmlLoginTicketRequest.SelectSingleNode("//uniqueId")!.InnerText =
                    _globalUniqueID.ToString();

                XmlLoginTicketRequest.SelectSingleNode("//generationTime")!.InnerText =
                    DateTime.UtcNow.AddMinutes(-10).ToString("yyyy-MM-ddTHH:mm:ss");

                XmlLoginTicketRequest.SelectSingleNode("//expirationTime")!.InnerText =
                    DateTime.UtcNow.AddMinutes(10).ToString("yyyy-MM-ddTHH:mm:ss");

                XmlLoginTicketRequest.SelectSingleNode("//service")!.InnerText =
                    servicio;

                Service = servicio;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{ID_FNC} Error GENERANDO LoginTicketRequest: {ex.Message}",
                    ex
                );
            }

            try
            {
                // ======================================================
                // PASO 2: FIRMAR LOGIN TICKET REQUEST
                // ======================================================
                X509Certificate2 certificado = CertificadosX509Lib
                    .ObtenerCertificadoDesdeArchivo(
                        rutaCertificadoP12,
                        passwordCertificado
                    );

                byte[] xmlBytes = Encoding.UTF8.GetBytes(
                    XmlLoginTicketRequest.OuterXml
                );

                byte[] cmsFirmado = CertificadosX509Lib
                    .FirmaBytesMensaje(xmlBytes, certificado);

                cmsFirmadoBase64 = Convert.ToBase64String(cmsFirmado);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{ID_FNC} Error FIRMANDO LoginTicketRequest: {ex.Message}",
                    ex
                );
            }

            try
            {
                // ======================================================
                // PASO 3: INVOCAR WSAA
                // ======================================================
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var wsaa = new WSAA.LoginCMSClient(
                    WSAA.LoginCMSClient.EndpointConfiguration.LoginCms
                );

                wsaa.Endpoint.Address =
                    new System.ServiceModel.EndpointAddress(urlWsaa);

                var response = wsaa
                    .loginCmsAsync(cmsFirmadoBase64)
                    .GetAwaiter()
                    .GetResult();

                loginTicketResponseXml = response.loginCmsReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{ID_FNC} Error INVOCANDO WSAA: {ex.Message}",
                    ex
                );
            }

            try
            {
                // ======================================================
                // PASO 4: PARSEAR RESPUESTA
                // ======================================================
                XmlLoginTicketResponse = new XmlDocument();
                XmlLoginTicketResponse.LoadXml(loginTicketResponseXml);
                return XmlLoginTicketResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{ID_FNC} Error ANALIZANDO LoginTicketResponse: {ex.Message}",
                    ex
                );
            }
        }
    }
}