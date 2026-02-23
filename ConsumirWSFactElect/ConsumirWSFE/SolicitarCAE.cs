using ConsumirWSFactElect.ConsumirWSFE.ClasesResponse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsumirWSFactElect.ConsumirWSFE
{
    public class SolicitarCAE
    {
        const string strUrlWsfev1 = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL";
        WSFEV1.Service servicioSolicitar; //objeto para solicitar el servicio wsfe
        TicketAcceso acceso = new TicketAcceso();
        //objetos solicitados por el método FECAESolicitar
        WSFEV1.FEAuthRequest auth;
        WSFEV1.FECAERequest peticion;//incluye cabecera y detalle
        WSFEV1.FECAECabRequest cabecera;
        WSFEV1.FECAEDetRequest detalle;
        WSFEV1.FECAEResponse response; //objeto mensaje de respuesta del método solicitar
        FEResponse respuesta;
       
        //solicita dos objetos - cabecera y detalle - para armar los respectivos objetos del ws
        //devuelve un objeto respuesta, el cual contiene cabecera, detalle (con el CAE), errores y eventos con la información a manipular
        public FEResponse solicitar(CabRequest cabReq, DetRequest detReq)
        {
            //solicito objeto de autenticación
            auth = acceso.ObtenerCredencialesTA();

            //FECAERequest arma objeto peticion
            try
            {
                peticion = new WSFEV1.FECAERequest();
                cabecera = new WSFEV1.FECAECabRequest();
                detalle = new WSFEV1.FECAEDetRequest();

                //armar objeto FECabRequest
                cabecera.CantReg = cabReq.cantReg; //cantidad de registros
                cabecera.PtoVta = cabReq.ptoVta; // punto de venta
                cabecera.CbteTipo = cabReq.cbteTipo; //tipo de comprobante

                //armar objeto FEDetRequest
                detalle.Concepto = detReq.concepto;
                detalle.DocTipo = detReq.docTipo;
                detalle.DocNro = detReq.docNro;
                detalle.CbteDesde = detReq.cbteDesde;
                detalle.CbteHasta = detReq.cbteHasta;
                detalle.CbteFch = detReq.cbteFch;
                detalle.ImpTotal = detReq.impTotal;
                detalle.ImpTotConc = detReq.impTotConc;
                detalle.ImpNeto = detReq.impNeto;
                detalle.ImpOpEx = detReq.impOpEx;
                detalle.ImpTrib = detReq.impTrib;
                detalle.ImpIVA = detReq.impIVA;
                detalle.MonId = detReq.monId;
                detalle.MonCotiz = detReq.monCotiz;

                //completa comprobantes asociados si los hay
                if (detReq.cbtesAsoc != null)
                {
                    detalle.CbtesAsoc = new WSFEV1.CbteAsoc[detReq.cbtesAsoc.Length];
                    for (int i = 0; i < detReq.cbtesAsoc.Length; i++)
                    {
                        WSFEV1.CbteAsoc cbtesAsoc = new WSFEV1.CbteAsoc();
                        cbtesAsoc.Cuit = detReq.cbtesAsoc[i].cuit;
                        cbtesAsoc.Nro = detReq.cbtesAsoc[i].nro;
                        cbtesAsoc.PtoVta = detReq.cbtesAsoc[i].ptoVta;
                        cbtesAsoc.Tipo = detReq.cbtesAsoc[i].tipo;
                        detalle.CbtesAsoc[i] = cbtesAsoc;
                    }
                }         

                //completa los tributos si los hay
                if (detReq.tributos != null)
                {
                    detalle.Tributos = new WSFEV1.Tributo[detReq.tributos.Length];
                    for (int i = 0; i < detReq.tributos.Length; i++)
                    {
                        WSFEV1.Tributo tributos = new WSFEV1.Tributo();
                        tributos.Id = detReq.tributos[i].id;
                        tributos.Desc = detReq.tributos[i].desc;
                        tributos.BaseImp = detReq.tributos[i].baseImp;
                        tributos.Alic = detReq.tributos[i].alic;
                        tributos.Importe = detReq.tributos[i].importe;
                        detalle.Tributos[i] = tributos;
                    }                   
                }

                //arma el objeto petición para la solicitud
                peticion.FeCabReq = cabecera;
                peticion.FeDetReq = new WSFEV1.FECAEDetRequest[1];
                peticion.FeDetReq[0] = detalle;
            }
            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WSFE : " + excepcionAlInvocarWsfe.Message);
            }

            //ejecuto el método solicitar del servicio
            try
            {
                servicioSolicitar = new WSFEV1.Service();
                servicioSolicitar.Url = strUrlWsfev1;
                //guardo el contenido de la respuesta
                response = servicioSolicitar.FECAESolicitar(auth,peticion);
            }
            catch (Exception excepcionAlInvocarWsaa)
            {
                throw new Exception("***Error INVOCANDO al servicio WSFE : " + excepcionAlInvocarWsaa.Message);
            }

            //creo el objeto para devolver
            respuesta = new FEResponse();

            //respuesta errores si no es nulo
            if (response.Errors != null)
            {
                respuesta.errores = new ErroresResponse[response.Errors.Length];
                for (int i = 0; i < response.Errors.Length; i++)
                {
                    respuesta.errores[i] = new ErroresResponse();
                    respuesta.errores[i].code = response.Errors[i].Code;
                    respuesta.errores[i].msg = response.Errors[i].Msg;
                }
            }

            //respuesta eventos si no es nulo
            if (response.Events != null)
            {
                respuesta.eventos = new EventosResponse[response.Events.Length];
                for (int i = 0; i < response.Events.Length; i++)
                {
                    respuesta.eventos[i] = new EventosResponse();
                    respuesta.eventos[i].code = response.Events[i].Code;
                    respuesta.eventos[i].msg = response.Events[i].Msg;
                }
            }

            //respuesta cabecera            
            respuesta.cabResp = new CabResponse();
            respuesta.cabResp.cuit = response.FeCabResp.Cuit;
            respuesta.cabResp.ptoVta = response.FeCabResp.PtoVta;
            respuesta.cabResp.cbteTipo = response.FeCabResp.CbteTipo;
            respuesta.cabResp.fchProceso = response.FeCabResp.FchProceso;
            respuesta.cabResp.cantReg = response.FeCabResp.CantReg;
            respuesta.cabResp.resultado = response.FeCabResp.Resultado;
            respuesta.cabResp.reproceso = response.FeCabResp.Reproceso;

            //respuesta detalles
            respuesta.detResp = new DetResponse();
            respuesta.detResp.concepto = response.FeDetResp[0].Concepto;
            respuesta.detResp.docTipo = response.FeDetResp[0].DocTipo;
            respuesta.detResp.docNro = response.FeDetResp[0].DocNro;
            respuesta.detResp.cbteDesde = response.FeDetResp[0].CbteDesde;
            respuesta.detResp.cbteHasta = response.FeDetResp[0].CbteHasta;
            respuesta.detResp.resultado = response.FeDetResp[0].Resultado;
            respuesta.detResp.cae = response.FeDetResp[0].CAE;
            respuesta.detResp.cbteFch = response.FeDetResp[0].CbteFch;
            respuesta.detResp.caeFchVto = response.FeDetResp[0].CAEFchVto;
            //completa las observaciones si las hay
            if (response.FeDetResp[0].Observaciones != null)
            {
                var numero = response.FeDetResp[0].Observaciones.Length;
                respuesta.detResp.observaciones = new ObsResponse[1]; //= new ObsResponse[numero];
                for (int i = 0; i < response.FeDetResp[0].Observaciones.Length; i++)
                {
                    respuesta.detResp.observaciones[i].code = response.FeDetResp[0].Observaciones[i].Code;
                    respuesta.detResp.observaciones[i].msg = response.FeDetResp[0].Observaciones[i].Msg;
                }
            }

            return respuesta;
        }

    }
}
