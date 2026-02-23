using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronica
{
    public class Consumir
    {
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
