using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronicaProd.ConsumirWSFE.ClasesResponse
{
    public class CabResponse
    {
        public long cuit { get; set; }
        public int ptoVta { get; set; }
        public int cbteTipo { get; set; }
        public string fchProceso { get; set; }
        public int cantReg { get; set; }
        public string resultado { get; set; }
        public string reproceso { get; set; }
    }
}
