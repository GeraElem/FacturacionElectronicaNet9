using ConsumirWSFactElect.ConsumirWSFE;
using ConsumirWSFactElect.ConsumirWSFE.ClasesResponse;
using ConsumirWSFactElect.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsumirWSFactElect
{
    class Program
    {

        static void Main(string[] args) //prueba para obtener el ticket de acceso
        {
            GetCotizacion();
        }

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
                if (i%2 == 0)
                    non += Convert.ToInt32(cae.Substring(i, 1));
                else
                    par += Convert.ToInt32(cae.Substring(i,1));
            }

            var sum = par + (non*3);

            for (int i = 0; i < 10; i++)
            {
                if (((sum + i)%10) == 0)
                {
                    return cae + Convert.ToString(i);
                }
            }
            return cae;
        }

        public static void GenerarFactura()
        {
            SolicitarCAE acceso = new SolicitarCAE();
            UltimoCbteAutorizado recuperarCbteAut = new UltimoCbteAutorizado();
            CbteAutResponse cbteAut = new CbteAutResponse();

            //ver ultimo cbte autorizado
            cbteAut = recuperarCbteAut.obtenerUltimoCbtAut(24, 11);
            Console.WriteLine(cbteAut.cbteNro);

            //ejemplo para cabecera
            CabRequest cabReq = new CabRequest();
            cabReq.cantReg = 1;
            cabReq.ptoVta = 24;
            cabReq.cbteTipo = 11;

            //ejemplo para detalle
            DetRequest detReq = new DetRequest();
            detReq.concepto = 1;
            detReq.docTipo = 80;
            detReq.docNro = 20111111112;
            detReq.cbteDesde = cbteAut.cbteNro + 1;
            detReq.cbteHasta = cbteAut.cbteNro + 1;
            detReq.cbteFch = "20180403";
            detReq.impTotal = 184.05;
            detReq.impTotConc = 0;
            detReq.impNeto = 184.05;
            detReq.impOpEx = 0;
            detReq.impTrib = 0;
            detReq.impIVA = 0;
            detReq.monId = "PES";
            detReq.monCotiz = 1;
            //ejemplo tributo
            /*TributoRequest tributos = new TributoRequest();
            tributos.id = 99;
            tributos.desc = "otros";
            tributos.baseImp = 0;
            tributos.alic = 0;
            tributos.importe = 34.05;
            detReq.tributos = new TributoRequest[1];
            detReq.tributos[0] = tributos;*/
            var resultado = acceso.solicitar(cabReq, detReq);
            Console.WriteLine(resultado.detResp.cae);
            Console.WriteLine(resultado.detResp.caeFchVto);
            var numerosCodigoBarra = resultado.cabResp.cuit + resultado.cabResp.cbteTipo.ToString("00") + resultado.cabResp.ptoVta.ToString("0000") + resultado.detResp.cae + resultado.detResp.caeFchVto;
            var codigoBarra = DigitoVerificador(numerosCodigoBarra);
            Console.WriteLine(codigoBarra);
            Console.ReadKey();
        }

        public static void PadronA5()
        {
            FacturaElectronicaProd.ConsumirWSFE.Padron acceso = new FacturaElectronicaProd.ConsumirWSFE.Padron();
            var resultado = acceso.ValidarCuit(20384074392);
            Console.WriteLine();
            Console.ReadKey();
        }

        public static void GetCotizacion()
        {
            FacturaElectronicaProd.ConsumirWSFE.FEParamGetCotizacion acceso = new FacturaElectronicaProd.ConsumirWSFE.FEParamGetCotizacion();
            var resultado = acceso.obenerCotizacion("DOL","20250413");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
