using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FacturaElectronicaProd.ConsumirWSFE;
using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;
//using FacturaElectronicaProd.WSFEV1;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class SolicitarCAE
    {
        //string strUrlWsfev1 = (System.Configuration.ConfigurationManager.AppSettings["AFIP"] == "P" ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL" : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL"); //produccion
        //WSFEV1.Service servicioSolicitar; //objeto para solicitar el servicio wsfe
        //TicketAcceso acceso = new TicketAcceso();
        ////objetos solicitados por el método FECAESolicitar
        //WSFEV1.FEAuthRequest auth;
        //WSFEV1.FECAERequest peticion;//incluye cabecera y detalle
        //WSFEV1.FECAECabRequest cabecera;
        //WSFEV1.FECAEDetRequest detalle;
        //WSFEV1.FECAEResponse response; //objeto mensaje de respuesta del método solicitar
        //FEResponse respuesta;
       
        ////solicita dos objetos - cabecera y detalle - para armar los respectivos objetos del ws
        ////devuelve un objeto respuesta, el cual contiene cabecera, detalle (con el CAE), errores y eventos con la información a manipular
        //public FEResponse solicitar(CabRequest cabReq, DetRequest detReq, string pathXml = null, string pathCertificado = null)
        //{
        //    //solicito objeto de autenticación
        //    auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

        //    //FECAERequest arma objeto peticion
        //    try
        //    {
        //        peticion = new WSFEV1.FECAERequest();
        //        cabecera = new WSFEV1.FECAECabRequest();
        //        detalle = new WSFEV1.FECAEDetRequest();

        //        //armar objeto FECabRequest
        //        cabecera.CantReg = cabReq.cantReg; //cantidad de registros
        //        cabecera.PtoVta = cabReq.ptoVta; // punto de venta
        //        cabecera.CbteTipo = cabReq.cbteTipo; //tipo de comprobante

        //        //armar objeto FEDetRequest
        //        detalle.Concepto = detReq.concepto;
        //        detalle.DocTipo = detReq.docTipo;
        //        detalle.DocNro = detReq.docNro;
        //        detalle.CbteDesde = detReq.cbteDesde;
        //        detalle.CbteHasta = detReq.cbteHasta;
        //        detalle.CbteFch = detReq.cbteFch;
        //        detalle.ImpTotal = detReq.impTotal;
        //        detalle.ImpTotConc = detReq.impTotConc;
        //        detalle.ImpNeto = detReq.impNeto;
        //        detalle.ImpOpEx = detReq.impOpEx;
        //        detalle.ImpTrib = detReq.impTrib;
        //        detalle.ImpIVA = detReq.impIVA;
        //        detalle.MonId = detReq.monId;
        //        detalle.MonCotiz = detReq.monCotiz;
        //        detalle.MonCotizSpecified = detReq.monCotizSpecified;
        //        detalle.FchServDesde = detReq.fchServDesde;
        //        detalle.FchServHasta = detReq.fchServHasta;
        //        detalle.FchVtoPago = detReq.fchVtoPago;
        //        detalle.CanMisMonExt = detReq.canMisMonExt;
        //        detalle.CondicionIVAReceptorId = detReq.condicionIVAReceptorId;

        //        //completa comprobantes asociados si los hay
        //        if (detReq.cbtesAsoc != null)
        //        {
        //            detalle.CbtesAsoc = new WSFEV1.CbteAsoc[detReq.cbtesAsoc.Length];
        //            for (int i = 0; i < detReq.cbtesAsoc.Length; i++)
        //            {
        //                WSFEV1.CbteAsoc cbtesAsoc = new WSFEV1.CbteAsoc();
        //                cbtesAsoc.Cuit = detReq.cbtesAsoc[i].cuit;
        //                cbtesAsoc.Nro = detReq.cbtesAsoc[i].nro;
        //                cbtesAsoc.PtoVta = detReq.cbtesAsoc[i].ptoVta;
        //                cbtesAsoc.Tipo = detReq.cbtesAsoc[i].tipo;
        //                detalle.CbtesAsoc[i] = cbtesAsoc;
        //            }
        //        }         

        //        //completa los tributos si los hay
        //        if (detReq.tributos != null)
        //        {
        //            detalle.Tributos = new WSFEV1.Tributo[detReq.tributos.Length];
        //            for (int i = 0; i < detReq.tributos.Length; i++)
        //            {
        //                WSFEV1.Tributo tributos = new WSFEV1.Tributo();
        //                tributos.Id = detReq.tributos[i].id;
        //                tributos.Desc = detReq.tributos[i].desc;
        //                tributos.BaseImp = detReq.tributos[i].baseImp;
        //                tributos.Alic = detReq.tributos[i].alic;
        //                tributos.Importe = detReq.tributos[i].importe;
        //                detalle.Tributos[i] = tributos;
        //            }
        //        }

        //        //completa IVA si los hay
        //        if (detReq.iva != null)
        //        {
        //            detalle.Iva = new WSFEV1.AlicIva[detReq.iva.Length];
        //            for (int i = 0; i < detReq.iva.Length; i++)
        //            {
        //                WSFEV1.AlicIva iva = new WSFEV1.AlicIva();
        //                iva.Id = detReq.iva[i].id;
        //                iva.BaseImp = detReq.iva[i].baseImp;
        //                iva.Importe = detReq.iva[i].importe;
        //                detalle.Iva[i] = iva;
        //            }
        //        }

        //        //completa Opciones si los hay
        //        if (detReq.opcional != null)
        //        {
        //            detalle.Opcionales = new WSFEV1.Opcional[detReq.opcional.Length];
        //            for (int i = 0; i < detReq.opcional.Length; i++)
        //            {
        //                WSFEV1.Opcional opcional = new WSFEV1.Opcional();
        //                opcional.Id = detReq.opcional[i].id;
        //                opcional.Valor = detReq.opcional[i].valor;
        //                detalle.Opcionales[i] = opcional;
        //            }
        //        }

        //        //arma el objeto petición para la solicitud
        //        peticion.FeCabReq = cabecera;
        //        peticion.FeDetReq = new WSFEV1.FECAEDetRequest[1];
        //        peticion.FeDetReq[0] = detalle;
        //    }
        //    catch (Exception excepcionAlInvocarWsfe)
        //    {
        //        throw new Exception("***Error INVOCANDO al servicio WSFE : " + excepcionAlInvocarWsfe.Message);
        //    }

        //    //ejecuto el método solicitar del servicio
        //    try
        //    {
        //        servicioSolicitar = new WSFEV1.Service();
        //        servicioSolicitar.Url = strUrlWsfev1;
        //        //guardo el contenido de la respuesta
        //        response = servicioSolicitar.FECAESolicitar(auth,peticion);
        //    }
        //    catch (Exception excepcionAlInvocarWsaa)
        //    {
        //        throw new Exception("***Error INVOCANDO al servicio WSFE : " + excepcionAlInvocarWsaa.Message);
        //    }

        //    //creo el objeto para devolver
        //    respuesta = new FEResponse {detResp = new List<DetResponse>()};
        //    //respuesta errores si no es nulo
        //    if (response.Errors != null)
        //    {
        //        respuesta.errores = new ErroresResponse[response.Errors.Length];
        //        for (int i = 0; i < response.Errors.Length; i++)
        //        {
        //            respuesta.errores[i] = new ErroresResponse
        //            {
        //                code = response.Errors[i].Code,
        //                msg = response.Errors[i].Msg
        //            };
        //        }
        //    }

        //    //respuesta eventos si no es nulo
        //    if (response.Events != null)
        //    {
        //        respuesta.eventos = new EventosResponse[response.Events.Length];
        //        for (int i = 0; i < response.Events.Length; i++)
        //        {
        //            respuesta.eventos[i] = new EventosResponse
        //            {
        //                code = response.Events[i].Code,
        //                msg = response.Events[i].Msg
        //            };
        //        }
        //    }

        //    //respuesta cabecera            
        //    respuesta.cabResp = new CabResponse
        //    {
        //        cuit = response.FeCabResp.Cuit,
        //        ptoVta = response.FeCabResp.PtoVta,
        //        cbteTipo = response.FeCabResp.CbteTipo,
        //        fchProceso = response.FeCabResp.FchProceso,
        //        cantReg = response.FeCabResp.CantReg,
        //        resultado = response.FeCabResp.Resultado,
        //        reproceso = response.FeCabResp.Reproceso
        //    };

        //    //respuesta detalles
        //    var respuestaDetalle = new DetResponse
        //    {
        //        concepto = response.FeDetResp[0].Concepto,
        //        docTipo = response.FeDetResp[0].DocTipo,
        //        docNro = response.FeDetResp[0].DocNro,
        //        cbteDesde = response.FeDetResp[0].CbteDesde,
        //        cbteHasta = response.FeDetResp[0].CbteHasta,
        //        resultado = response.FeDetResp[0].Resultado,
        //        cae = response.FeDetResp[0].CAE,
        //        cbteFch = response.FeDetResp[0].CbteFch,
        //        caeFchVto = response.FeDetResp[0].CAEFchVto
        //    };
        //    //completa las observaciones si las hay
        //    if (response.FeDetResp[0].Observaciones != null)
        //    {
        //        respuestaDetalle.observaciones = new ObsResponse[response.FeDetResp[0].Observaciones.Length];
        //        for (int i = 0; i < response.FeDetResp[0].Observaciones.Length; i++)
        //        {
        //            respuestaDetalle.observaciones[i] = new ObsResponse
        //            {
        //                code = response.FeDetResp[0].Observaciones[i].Code,
        //                msg = response.FeDetResp[0].Observaciones[i].Msg
        //            };
        //        }
        //    }

        //    respuesta.detResp.Add(respuestaDetalle);

        //    return respuesta;
        //}

        ////solicita dos objetos - cabecera y detalle - para armar los respectivos objetos del ws
        ////devuelve un objeto respuesta, el cual contiene cabecera, detalle (con el CAE), errores y eventos con la información a manipular
        //public FEResponse solicitarMultiples(CabRequest cabReq, List<DetRequest> detReq, string pathXml = null, string pathCertificado = null)
        //{
        //    try
        //    {
        //        //solicito objeto de autenticación
        //        auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

        //        //FECAERequest arma objeto peticion
        //        try
        //        {
        //            peticion = new WSFEV1.FECAERequest();
        //            cabecera = new WSFEV1.FECAECabRequest();

        //            //armar objeto FECabRequest
        //            cabecera.CantReg = cabReq.cantReg; //cantidad de registros
        //            cabecera.PtoVta = cabReq.ptoVta; // punto de venta
        //            cabecera.CbteTipo = cabReq.cbteTipo; //tipo de comprobante

        //            //arma el objeto petición para la solicitud
        //            peticion.FeCabReq = cabecera;
        //            peticion.FeDetReq = new WSFEV1.FECAEDetRequest[detReq.Count];

        //            //armar objeto FEDetRequest
        //            var index = 0;
        //            foreach (var item in detReq)
        //            {
        //                detalle = new FECAEDetRequest
        //                {
        //                    Concepto = item.concepto,
        //                    DocTipo = item.docTipo,
        //                    DocNro = item.docNro,
        //                    CbteDesde = item.cbteDesde,
        //                    CbteHasta = item.cbteHasta,
        //                    CbteFch = item.cbteFch,
        //                    ImpTotal = item.impTotal,
        //                    ImpTotConc = item.impTotConc,
        //                    ImpNeto = item.impNeto,
        //                    ImpOpEx = item.impOpEx,
        //                    ImpTrib = item.impTrib,
        //                    ImpIVA = item.impIVA,
        //                    MonId = item.monId,
        //                    MonCotiz = item.monCotiz,
        //                    MonCotizSpecified=item.monCotizSpecified,
        //                    CondicionIVAReceptorId=item.condicionIVAReceptorId,
        //                    CanMisMonExt =item.canMisMonExt,
        //                };

        //                //completa comprobantes asociados si los hay
        //                if (item.cbtesAsoc != null)
        //                {
        //                    detalle.CbtesAsoc = new WSFEV1.CbteAsoc[item.cbtesAsoc.Length];
        //                    for (int i = 0; i < item.cbtesAsoc.Length; i++)
        //                    {
        //                        WSFEV1.CbteAsoc cbtesAsoc = new WSFEV1.CbteAsoc();
        //                        cbtesAsoc.Cuit = item.cbtesAsoc[i].cuit;
        //                        cbtesAsoc.Nro = item.cbtesAsoc[i].nro;
        //                        cbtesAsoc.PtoVta = item.cbtesAsoc[i].ptoVta;
        //                        cbtesAsoc.Tipo = item.cbtesAsoc[i].tipo;
        //                        detalle.CbtesAsoc[i] = cbtesAsoc;
        //                    }
        //                }

        //                //completa los tributos si los hay
        //                if (item.tributos != null)
        //                {
        //                    detalle.Tributos = new WSFEV1.Tributo[item.tributos.Length];
        //                    for (int i = 0; i < item.tributos.Length; i++)
        //                    {
        //                        WSFEV1.Tributo tributos = new WSFEV1.Tributo();
        //                        tributos.Id = item.tributos[i].id;
        //                        tributos.Desc = item.tributos[i].desc;
        //                        tributos.BaseImp = item.tributos[i].baseImp;
        //                        tributos.Alic = item.tributos[i].alic;
        //                        tributos.Importe = item.tributos[i].importe;
        //                        detalle.Tributos[i] = tributos;
        //                    }
        //                }

        //                peticion.FeDetReq[index] = detalle;
        //                index++;
        //            }
        //        }
        //        catch (Exception excepcionAlInvocarWsfe)
        //        {
        //            throw new Exception("***Error INVOCANDO al servicio WSFE : " + excepcionAlInvocarWsfe.Message);
        //        }

        //        //ejecuto el método solicitar del servicio
        //        try
        //        {
        //            servicioSolicitar = new WSFEV1.Service();
        //            servicioSolicitar.Url = strUrlWsfev1;
        //            //guardo el contenido de la respuesta
        //            response = servicioSolicitar.FECAESolicitar(auth, peticion);
        //        }
        //        catch (Exception excepcionAlInvocarWsaa)
        //        {
        //            throw new Exception("***Error INVOCANDO al servicio WSFE : " + excepcionAlInvocarWsaa.Message);
        //        }

        //        //creo el objeto para devolver
        //        respuesta = new FEResponse();

        //        //respuesta cabecera            
        //        respuesta.cabResp = new CabResponse
        //        {
        //            cuit = response.FeCabResp.Cuit,
        //            ptoVta = response.FeCabResp.PtoVta,
        //            cbteTipo = response.FeCabResp.CbteTipo,
        //            fchProceso = response.FeCabResp.FchProceso,
        //            cantReg = response.FeCabResp.CantReg,
        //            resultado = response.FeCabResp.Resultado,
        //            reproceso = response.FeCabResp.Reproceso
        //        };

        //        //respuesta errores si no es nulo
        //        if (response.Errors != null)
        //        {
        //            respuesta.errores = new ErroresResponse[response.Errors.Length];
        //            for (int i = 0; i < response.Errors.Length; i++)
        //            {
        //                respuesta.errores[i] = new ErroresResponse
        //                {
        //                    code = response.Errors[i].Code,
        //                    msg = response.Errors[i].Msg
        //                };
        //            }
        //        }

        //        //respuesta eventos si no es nulo
        //        if (response.Events != null)
        //        {
        //            respuesta.eventos = new EventosResponse[response.Events.Length];
        //            for (int i = 0; i < response.Events.Length; i++)
        //            {
        //                respuesta.eventos[i] = new EventosResponse
        //                {
        //                    code = response.Events[i].Code,
        //                    msg = response.Events[i].Msg
        //                };
        //            }
        //        }

        //        //respuesta detalles
        //        respuesta.detResp = new List<DetResponse>();
        //        for (int i = 0; i < response.FeDetResp.Count(); i++)
        //        {
        //            //respuesta detalles
        //            var respuestaDetalle = new DetResponse
        //            {
        //                concepto = response.FeDetResp[i].Concepto,
        //                docTipo = response.FeDetResp[i].DocTipo,
        //                docNro = response.FeDetResp[i].DocNro,
        //                cbteDesde = response.FeDetResp[i].CbteDesde,
        //                cbteHasta = response.FeDetResp[i].CbteHasta,
        //                resultado = response.FeDetResp[i].Resultado,
        //                cae = response.FeDetResp[i].CAE,
        //                cbteFch = response.FeDetResp[i].CbteFch,
        //                caeFchVto = response.FeDetResp[i].CAEFchVto
        //            };

        //            //completa las observaciones si las hay
        //            if (response.FeDetResp[i].Observaciones != null)
        //            {
        //                var numero = response.FeDetResp[i].Observaciones.Length;
        //                respuestaDetalle.observaciones = new ObsResponse[numero]; //= new ObsResponse[numero];
        //                for (int x = 0; x < response.FeDetResp[i].Observaciones.Length; x++)
        //                {
        //                    respuestaDetalle.observaciones[x] = new ObsResponse
        //                    {
        //                        code = response.FeDetResp[i].Observaciones[x].Code,
        //                        msg = response.FeDetResp[i].Observaciones[x].Msg
        //                    };
        //                }
        //            }

        //            respuesta.detResp.Add(respuestaDetalle);
        //        }

        //        return respuesta;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static string DigitoVerificador(string cae)
        {
            //EL CODIGO DE BARRA SON:
            //11 DIGITOS DEL CUIT DE LA EMPRESA QUE EMITE LA FACTURA
            //2 DIGITOS DEL TIPO DE COMPROBANTE
            //4 DIGITOS DEL PUNTO DE VENTA
            //LOS DIGITOS QUE DEVUELVE EL CAE
            //LA FECHA DE VENCIMIENTO DEL CAE EN FORMATO AAAAMMDD
            //Y AL FINAL SE UBICA EL DIGITO VERIFICADOR
            var par = 0;
            var non = 0;

            for (int i = 0; i < cae.Length; i++)
            {
                //i%2 te devuelve el resto de la division
                if (i % 2 == 0)
                    non += Convert.ToInt32(cae.Substring(i, 1));
                else
                    par += Convert.ToInt32(cae.Substring(i, 1));
            }

            var sum = par + (non * 3);

            for (int i = 0; i < 10; i++)
            {
                if (((sum + i) % 10) == 0)
                {
                    return cae + Convert.ToString(i);
                }
            }
            return cae;
        }

    }
}
