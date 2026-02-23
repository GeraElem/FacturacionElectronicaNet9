using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronicaProd.ConsumirWSFE.ClasesRequest
{
    public class TributoRequest
    {
        public short id { get; set; }
        public string desc { get; set; }
        public double baseImp { get; set; }
        public double alic { get; set; }
        public double importe { get; set; }
    }
}
