using System;
using System.ServiceModel;
using FacturaElectronicaProd.ServiceReference2;
using PadronService;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class Padron
    {
        private readonly PersonaServiceA5Client _cliente;
        private readonly TicketAcceso _acceso = new TicketAcceso();

        private const string strIdServicioNegocio = "ws_sr_constancia_inscripcion";

        public Padron(string urlWs)
        {
            var endpoint = new EndpointAddress(urlWs);

            _cliente = new PersonaServiceA5Client(
                PersonaServiceA5Client.EndpointConfiguration.PersonaServiceA5Port,
                endpoint
            );
        }

        public personaReturn ValidarCuit(long cuit)
        {
            try
            {
                // 1️⃣ Obtener credenciales WSAA
                FEAuthRequest auth = _acceso.PadronObtenerCredencialesTA(strIdServicioNegocio);

                // 2️⃣ Invocar servicio Padron A5
                var resultado = _cliente
                    .getPersona_v2Async(
                        auth.Token,
                        auth.Sign,
                        auth.Cuit,
                        cuit
                    )
                    .GetAwaiter()
                    .GetResult();

                // 3️⃣ Devolver persona
                return resultado.personaReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "*** Error invocando servicio Padron A5: " + ex.Message,
                    ex
                );
            }
        }
    }
}
