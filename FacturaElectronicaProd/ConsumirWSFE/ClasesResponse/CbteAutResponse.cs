using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronicaProd.ConsumirWSFE.ClasesResponse
{
    public class CbteAutResponse
    {
        public ErroresResponse[] errores { get; set; }
        public EventosResponse[] eventos { get; set; }
        public int cbteNro { get; set; }
        public int cbteTipo { get; set; }
        public int ptoVta { get; set; }
    }
}
