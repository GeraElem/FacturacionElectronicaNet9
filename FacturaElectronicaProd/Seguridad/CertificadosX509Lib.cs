using System;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace FacturaElectronicaProd.Seguridad
{
    internal static class CertificadosX509Lib
    {
        // ======================================================
        // FIRMA MENSAJE PKCS#7
        // ======================================================
        public static byte[] FirmaBytesMensaje(byte[] mensaje, X509Certificate2 certificadoFirmante)
        {
            const string ID_FNC = "[FirmaBytesMensaje]";

            try
            {
                if (!certificadoFirmante.HasPrivateKey)
                {
                    throw new Exception("El certificado no contiene clave privada.");
                }

                ContentInfo contentInfo = new ContentInfo(mensaje);
                SignedCms signedCms = new SignedCms(contentInfo, detached: false);

                CmsSigner signer = new CmsSigner(certificadoFirmante)
                {
                    IncludeOption = X509IncludeOption.EndCertOnly
                };

                signedCms.ComputeSignature(signer);
                return signedCms.Encode();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{ID_FNC} Error al firmar mensaje: {ex.Message}",
                    ex
                );
            }
        }

        // ======================================================
        // CARGA CERTIFICADO DESDE ARCHIVO (.p12 / .pfx)
        // ======================================================
        public static X509Certificate2 ObtenerCertificadoDesdeArchivo(
            string rutaCertificado,
            string password = null)
        {
            const string ID_FNC = "[ObtenerCertificadoDesdeArchivo]";

            try
            {
                if (!System.IO.File.Exists(rutaCertificado))
                {
                    throw new Exception("El archivo de certificado no existe.");
                }

                return new X509Certificate2(
                    rutaCertificado,
                    password);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"{ID_FNC} Error al leer certificado: {ex.Message} ---- {rutaCertificado}",
                    ex
                );
            }
        }
    }
}