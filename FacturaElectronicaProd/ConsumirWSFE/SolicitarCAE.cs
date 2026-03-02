using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;
using FacturaElectronicaProd.ConsumirWSFE.ClasesResponse;
using FacturaElectronicaProd.ServiceReference2;
using Microsoft.Extensions.Configuration;

namespace FacturaElectronicaProd.ConsumirWSFE
{
    public class SolicitarCAE
    {
        private readonly string strUrlWsfev1;
        private readonly TicketAcceso acceso;
        private ServiceSoapClient servicioSolicitar;

        public SolicitarCAE(IConfiguration configuration)
        {
            var ambiente = configuration.GetValue<string>("AFIP");

            strUrlWsfev1 = ambiente == "P"
                ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL"
                : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL";

            acceso = new TicketAcceso(configuration);
        }

        #region Solicitar CAE simple

        public FEResponse Solicitar(
            CabRequest cabReq,
            DetRequest detReq,
            string pathXml = null,
            string pathCertificado = null)
        {
            // 1️⃣ Autenticación
            var auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

            // 2️⃣ Armar petición AFIP
            var peticion = new FECAERequest
            {
                FeCabReq = new FECAECabRequest
                {
                    CantReg = cabReq.cantReg,
                    PtoVta = cabReq.ptoVta,
                    CbteTipo = cabReq.cbteTipo
                },
                FeDetReq = new[]
                {
                    ArmarDetalle(detReq)
                }
            };

            // 3️⃣ Cliente WCF
            var endpoint = new EndpointAddress(strUrlWsfev1);
            servicioSolicitar = new ServiceSoapClient(
                ServiceSoapClient.EndpointConfiguration.ServiceSoap,
                endpoint);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // 4️⃣ Llamada WS
            var response = servicioSolicitar
                .FECAESolicitarAsync(auth, peticion)
                .GetAwaiter()
                .GetResult();

            var result = response.Body.FECAESolicitarResult;

            // 5️⃣ Mapear respuesta
            return MapearRespuesta(result);
        }

        #endregion

        #region Solicitar CAE múltiples

        public FEResponse SolicitarMultiples(
            CabRequest cabReq,
            List<DetRequest> detalles,
            string pathXml = null,
            string pathCertificado = null)
        {
            var auth = acceso.ObtenerCredencialesTA(pathXml, pathCertificado);

            var peticion = new FECAERequest
            {
                FeCabReq = new FECAECabRequest
                {
                    CantReg = cabReq.cantReg,
                    PtoVta = cabReq.ptoVta,
                    CbteTipo = cabReq.cbteTipo
                },
                FeDetReq = detalles
                    .Select(ArmarDetalle)
                    .ToArray()
            };

            var endpoint = new EndpointAddress(strUrlWsfev1);
            servicioSolicitar = new ServiceSoapClient(
                ServiceSoapClient.EndpointConfiguration.ServiceSoap,
                endpoint);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var response = servicioSolicitar
                .FECAESolicitarAsync(auth, peticion)
                .GetAwaiter()
                .GetResult();

            var result = response.Body.FECAESolicitarResult;

            return MapearRespuesta(result);
        }

        #endregion

        #region Helpers

        private FECAEDetRequest ArmarDetalle(DetRequest item)
        {
            var detalle = new FECAEDetRequest
            {
                Concepto = item.concepto,
                DocTipo = item.docTipo,
                DocNro = item.docNro,
                CbteDesde = item.cbteDesde,
                CbteHasta = item.cbteHasta,
                CbteFch = item.cbteFch,
                ImpTotal = item.impTotal,
                ImpTotConc = item.impTotConc,
                ImpNeto = item.impNeto,
                ImpOpEx = item.impOpEx,
                ImpTrib = item.impTrib,
                ImpIVA = item.impIVA,
                MonId = item.monId,
                MonCotiz = item.monCotiz,
                //MonCotizSpecified = item.monCotizSpecified,
                CondicionIVAReceptorId = item.condicionIVAReceptorId,
                CanMisMonExt = item.canMisMonExt
            };

            if (item.cbtesAsoc != null)
            {
                detalle.CbtesAsoc = item.cbtesAsoc
                    .Select(c => new CbteAsoc
                    {
                        Cuit = c.cuit,
                        Nro = c.nro,
                        PtoVta = c.ptoVta,
                        Tipo = c.tipo
                    })
                    .ToArray();
            }

            if (item.tributos != null)
            {
                detalle.Tributos = item.tributos
                    .Select(t => new Tributo
                    {
                        Id = t.id,
                        Desc = t.desc,
                        BaseImp = t.baseImp,
                        Alic = t.alic,
                        Importe = t.importe
                    })
                    .ToArray();
            }

            if (item.iva != null)
            {
                detalle.Iva = item.iva
                    .Select(i => new AlicIva
                    {
                        Id = i.id,
                        BaseImp = i.baseImp,
                        Importe = i.importe
                    })
                    .ToArray();
            }

            if (item.opcional != null)
            {
                detalle.Opcionales = item.opcional
                    .Select(o => new Opcional
                    {
                        Id = o.id,
                        Valor = o.valor
                    })
                    .ToArray();
            }

            return detalle;
        }

        private FEResponse MapearRespuesta(FECAEResponse result)
        {
            var respuesta = new FEResponse
            {
                cabResp = new CabResponse
                {
                    cuit = result.FeCabResp.Cuit,
                    ptoVta = result.FeCabResp.PtoVta,
                    cbteTipo = result.FeCabResp.CbteTipo,
                    fchProceso = result.FeCabResp.FchProceso,
                    cantReg = result.FeCabResp.CantReg,
                    resultado = result.FeCabResp.Resultado,
                    reproceso = result.FeCabResp.Reproceso
                },
                detResp = new List<DetResponse>()
            };

            if (result.Errors != null)
            {
                respuesta.errores = result.Errors
                    .Select(e => new ErroresResponse
                    {
                        code = e.Code,
                        msg = e.Msg
                    })
                    .ToArray();
            }

            if (result.Events != null)
            {
                respuesta.eventos = result.Events
                    .Select(e => new EventosResponse
                    {
                        code = e.Code,
                        msg = e.Msg
                    })
                    .ToArray();
            }

            foreach (var d in result.FeDetResp)
            {
                var det = new DetResponse
                {
                    concepto = d.Concepto,
                    docTipo = d.DocTipo,
                    docNro = d.DocNro,
                    cbteDesde = d.CbteDesde,
                    cbteHasta = d.CbteHasta,
                    resultado = d.Resultado,
                    cae = d.CAE,
                    cbteFch = d.CbteFch,
                    caeFchVto = d.CAEFchVto
                };

                if (d.Observaciones != null)
                {
                    det.observaciones = d.Observaciones
                        .Select(o => new ObsResponse
                        {
                            code = o.Code,
                            msg = o.Msg
                        })
                        .ToArray();
                }

                respuesta.detResp.Add(det);
            }

            return respuesta;
        }

        #endregion
    }
}