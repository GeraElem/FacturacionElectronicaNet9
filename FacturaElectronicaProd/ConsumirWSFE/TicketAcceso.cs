using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FacturaElectronicaProd.Seguridad;
using System.Web;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    class TicketAcceso
    {
        //// Valores por defecto, globales en esta clase
        //string strUrlWsaaWsdl = (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "https://wsaa.afip.gov.ar/ws/services/LoginCms?WSDL" : "https://wsaahomo.afip.gov.ar/ws/services/LoginCms?WSDL");
        //const string strIdServicioNegocio = "wsfe"; //Servicio a consumir
        //string strRutaCertSigner = HttpContext.Current.Server.MapPath("~/FE/") + (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "ClavePrivadaProd.p12" : "ClavePrivada.p12"); //Ruta del certificado P12
        ////string strRutaCertSigner = "C:\\FE\\" + (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "ClavePrivadaProd.p12" : "ClavePrivada.p12"); //Ruta del certificado P12
        //string strProxy = null;
        //string strProxyUser = null;
        //string strProxyPassword = null;
        //private LoginTicket ticket;
        ////string archivoXml = "C:\\FE\\" + (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "dataProd.xml" : "dataProd.xml");//archivo de almacentamiento del ticket de acceso
        ////string archivoXml_padron = "C:\\FE\\" + (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "dataPadronProd.xml" : "dataPadron.xml");//archivo de almacentamiento del ticket de acceso
        //string archivoXml = HttpContext.Current.Server.MapPath("~/FE/") + (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "dataProd.xml" : "data.xml");//archivo de almacentamiento del ticket de acceso
        //string archivoXml_padron = HttpContext.Current.Server.MapPath("~/FE/") + (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "dataPadronProd.xml" : "dataPadron.xml");//archivo de almacentamiento del ticket de acceso
        //XmlDocument xmlTicket;

        //public TicketAcceso() {
        //    xmlTicket = null;
        //}

        //public void generarTicketAcceso(string pathXml, string pathCertificado)
        //{
        //    const string ID_FNC = "[Main]";
        //    archivoXml = string.IsNullOrEmpty(pathXml) ? archivoXml : pathXml;
        //    strRutaCertSigner = string.IsNullOrEmpty(pathCertificado) ? strRutaCertSigner : pathCertificado;
        //    try
        //    {
        //        ticket = new LoginTicket();
        //        xmlTicket = ticket.ObtenerLoginTicketResponse(strIdServicioNegocio, strUrlWsaaWsdl, strRutaCertSigner, strProxy, strProxyUser, strProxyPassword);
        //    }
        //    catch (Exception excepcionAlObtenerTicket)
        //    {
        //        throw new Exception(ID_FNC + "***EXCEPCION AL OBTENER TICKET: " + excepcionAlObtenerTicket.Message);
        //    }
        //    //guardo el archivo
        //    xmlTicket.Save(archivoXml);
        //}
        
        ////devuelve un arraylist para la seccion de autenticacion de las peticiones
        //public WSFEV1.FEAuthRequest ObtenerCredencialesTA(string pathXml, string pathCertificado)
        //{
        //    try
        //    {
        //        archivoXml = string.IsNullOrEmpty(pathXml) ? archivoXml : pathXml;
        //        pathCertificado = string.IsNullOrEmpty(pathCertificado) ? strRutaCertSigner : pathCertificado;

        //        WSFEV1.FEAuthRequest objAuth = new WSFEV1.FEAuthRequest();
        //        //ArrayList credenciales = new ArrayList();
        //        DateTime localDate = DateTime.Now; //fecha y hora actual
        //        DateTime expirationTime; // Momento en el que expira la solicitud
        //        xmlTicket = new XmlDocument(); //para extraer la info del archivo xml con las credenciales
        //        if (!File.Exists(archivoXml))
        //        {
        //            //si el archivo que aloja el TA no existe, lo creo
        //            this.generarTicketAcceso(archivoXml,pathCertificado);
        //        }
        //        xmlTicket.Load(archivoXml);
        //        //guardo el tiempo de expiración del ticket de acceso para verificar 
        //        expirationTime = DateTime.Parse(xmlTicket.SelectSingleNode("//expirationTime").InnerText);
        //        int result = DateTime.Compare(expirationTime, localDate); //compara las fechas
        //        //menor que 0= expirado, mayor que 0= aun no expiró
        //        if (result <= 0) //si expiró lo vuelvo a generar
        //        {
        //            this.generarTicketAcceso(archivoXml, pathCertificado);
        //        }
        //        //extraigo sign y token del archivo y guardo en arraylist
        //        objAuth.Token = xmlTicket.SelectSingleNode("//token").InnerText;
        //        objAuth.Sign = xmlTicket.SelectSingleNode("//sign").InnerText;
        //        //extraigo el cuit del xml
        //        String value = xmlTicket.SelectSingleNode("//destination").InnerText;
        //        int startIndex = 18;
        //        int length = 11;
        //        String cuit = value.Substring(startIndex, length);
        //        //agrego al objeto de autenticación
        //        objAuth.Cuit = long.Parse(cuit);
        //        return objAuth;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void PadronGenerarTicketAcceso(string servicio)
        //{
        //    const string ID_FNC = "[Main]";
        //    try
        //    {
        //        ticket = new LoginTicket();
        //        xmlTicket = ticket.ObtenerLoginTicketResponse(servicio, strUrlWsaaWsdl, strRutaCertSigner, strProxy, strProxyUser, strProxyPassword);
        //    }
        //    catch (Exception excepcionAlObtenerTicket)
        //    {
        //        throw new Exception(ID_FNC + "***EXCEPCION AL OBTENER TICKET: " + excepcionAlObtenerTicket.Message);
        //    }
        //    //guardo el archivo
        //    xmlTicket.Save(archivoXml_padron);
        //}

        //public WSFEV1.FEAuthRequest PadronObtenerCredencialesTA(string servicio)
        //{
        //    try
        //    {
        //        WSFEV1.FEAuthRequest objAuth = new WSFEV1.FEAuthRequest();
        //        //ArrayList credenciales = new ArrayList();
        //        DateTime localDate = DateTime.Now; //fecha y hora actual
        //        DateTime expirationTime; // Momento en el que expira la solicitud
        //        xmlTicket = new XmlDocument(); //para extraer la info del archivo xml con las credenciales
        //        if (!File.Exists(archivoXml_padron))
        //        {
        //            //si el archivo que aloja el TA no existe, lo creo
        //            this.PadronGenerarTicketAcceso(servicio);
        //        }
        //        xmlTicket.Load(archivoXml_padron);
        //        //guardo el tiempo de expiración del ticket de acceso para verificar 
        //        expirationTime = DateTime.Parse(xmlTicket.SelectSingleNode("//expirationTime").InnerText);
        //        int result = DateTime.Compare(expirationTime, localDate); //compara las fechas
        //        //menor que 0= expirado, mayor que 0= aun no expiró
        //        if (result <= 0) //si expiró lo vuelvo a generar
        //        {
        //            this.PadronGenerarTicketAcceso(servicio);
        //        }
        //        //extraigo sign y token del archivo y guardo en arraylist
        //        objAuth.Token = xmlTicket.SelectSingleNode("//token").InnerText;
        //        objAuth.Sign = xmlTicket.SelectSingleNode("//sign").InnerText;
        //        //extraigo el cuit del xml
        //        String value = xmlTicket.SelectSingleNode("//destination").InnerText;
        //        int startIndex = 18;
        //        int length = 11;
        //        String cuit = value.Substring(startIndex, length);
        //        //agrego al objeto de autenticación
        //        objAuth.Cuit = long.Parse(cuit);
        //        return objAuth;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
