using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacturaElectronica.ConsumirWSFE.ClasesRequest;
using FacturaElectronica.ConsumirWSFE.ClasesResponse;

namespace FacturaElectronica.ConsumirWSFE
{
    public class FECompConsultar
    {
        const string strUrlWsfev1 = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL";
        //const string strUrlWsfev1 = "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL"; //produccion
        WSFEV1.Service servicioCompCons; //objeto para solicitar el servicio wsfe
        TicketAcceso acceso = new TicketAcceso();
        //objetos solicitados por el método FECompConsultar
        WSFEV1.FEAuthRequest auth;
        WSFEV1.FECompConsultaResponse response;

        //objetos de la DLL
        FECompConsultarResult respuesta;
        private FECompConsultarResult comprobante;


        public FECompConsultarResult ConsultarComprobante(FeCompConsReq cabReq)
        {
            //solicito objeto de autenticación
            auth = acceso.ObtenerCredencialesTA();

            //completo el objeto de request
            var peticion = new WSFEV1.FECompConsultaReq
            {
                CbteNro = cabReq.CbteNro,
                CbteTipo = cabReq.CbteTipo,
                PtoVta = cabReq.PtoVta
            };
            
            //ejecuto el método compUltimoAutorizado del servicio
            try
            {
                servicioCompCons = new WSFEV1.Service();
                servicioCompCons.Url = strUrlWsfev1;
                //guardo el contenido de la respuesta
                response = servicioCompCons.FECompConsultar(auth, peticion);
            }
            catch (Exception excepcionAlInvocarWsaa)
            {
                throw new Exception("***Error INVOCANDO al servicio WSFE : " + excepcionAlInvocarWsaa.Message);
            }

            //creo el objeto a devolver
            respuesta = new FECompConsultarResult();

            respuesta.comprobante = new FECompConsultarResponse
            {
                concepto = response.ResultGet.Concepto,
                docTipo = response.ResultGet.DocTipo,
                docNro = response.ResultGet.DocNro,
                cbteDesde = response.ResultGet.CbteDesde,
                cbteHasta = response.ResultGet.CbteHasta,
                cbteFch = response.ResultGet.CbteFch,
                impTotal = response.ResultGet.ImpTotal,
                impTotConc = response.ResultGet.ImpTotConc,
                impNeto = response.ResultGet.ImpNeto,
                impOpEx = response.ResultGet.ImpOpEx,
                impTrib = response.ResultGet.ImpTrib,
                impIVA = response.ResultGet.ImpIVA,
                monId = response.ResultGet.MonId,
                monCotiz = response.ResultGet.MonCotiz,
                resultado = response.ResultGet.Resultado,
                cae = response.ResultGet.CodAutorizacion,
                fechaProceso = response.ResultGet.FchProceso,
                ptoVta = response.ResultGet.PtoVta,
                cbteTipo = response.ResultGet.CbteTipo,
            };

            //respuesta errores si no es nulo
            if (response.Errors != null)
            {
                respuesta.errores = new ErroresResponse[response.Errors.Length];
                for (int i = 0; i < response.Errors.Length; i++)
                {
                    respuesta.errores[i] = new ErroresResponse
                    {
                        code = response.Errors[i].Code,
                        msg = response.Errors[i].Msg
                    };
                }
            }

            //respuesta eventos si no es nulo
            if (response.Events != null)
            {
                respuesta.eventos = new EventosResponse[response.Events.Length];
                for (int i = 0; i < response.Events.Length; i++)
                {
                    respuesta.eventos[i] = new EventosResponse
                    {
                        code = response.Events[i].Code,
                        msg = response.Events[i].Msg
                    };
                }
            }

            return respuesta;
        }
    }
}
