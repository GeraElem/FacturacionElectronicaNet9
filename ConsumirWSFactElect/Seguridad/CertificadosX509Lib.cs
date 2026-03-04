using System;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace ConsumirWSFactElect.Seguridad
{
    class CertificadosX509Lib
    {
        // 🔐 Firma mensaje
        public static byte[] FirmaBytesMensaje(byte[] argBytesMsg, X509Certificate2 argCertFirmante)
        {
            const string ID_FNC = "[FirmaBytesMensaje]";
            try
            {
                var infoContenido = new ContentInfo(argBytesMsg);
                var cmsFirmado = new SignedCms(infoContenido);

                var cmsFirmante = new CmsSigner(argCertFirmante)
                {
                    IncludeOption = X509IncludeOption.EndCertOnly
                };

                cmsFirmado.ComputeSignature(cmsFirmante);
                return cmsFirmado.Encode();
            }
            catch (Exception ex)
            {
                throw new Exception(ID_FNC + "***Error al firmar: " + ex.Message, ex);
            }
        }

        // 📄 Lee certificado .p12 / .pfx (NET 6+ compatible)
        public static X509Certificate2 ObtieneCertificadoDesdeArchivo(
            string pathCertificado,
            string password)
        {
            const string ID_FNC = "[ObtieneCertificadoDesdeArchivo]";
            try
            {
                var flags =
                    RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        ? X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable
                        : X509KeyStorageFlags.EphemeralKeySet;

                return new X509Certificate2(
                    pathCertificado,
                    password,
                    flags
                );
            }
            catch (Exception ex)
            {
                throw new Exception(
                    ID_FNC + "***Error al leer certificado: " + ex.Message,
                    ex
                );
            }
        }
    }
}