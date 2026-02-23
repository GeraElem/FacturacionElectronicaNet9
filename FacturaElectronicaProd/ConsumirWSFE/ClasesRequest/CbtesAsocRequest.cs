using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronicaProd.ConsumirWSFE.ClasesRequest
{
    public class CbtesAsocRequest
    {
        public string cuit { get; set; }
        public long nro { get; set; }
        public int ptoVta { get; set; }
        public int tipo { get; set; }
    }
}
