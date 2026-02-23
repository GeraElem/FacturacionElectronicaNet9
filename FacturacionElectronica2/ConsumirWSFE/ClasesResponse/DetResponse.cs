using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronica.ConsumirWSFE.ClasesResponse
{
    public class DetResponse
    {
        public int concepto { get; set; }
        public int docTipo { get; set; }
        public long docNro { get; set; }
        public long cbteDesde { get; set; }
        public long cbteHasta { get; set; }
        public string resultado { get; set; }
        public string cae { get; set; }
        public string cbteFch { get; set; }
        public string caeFchVto { get; set; }
        public ObsResponse[] observaciones { get; set; }
    }
}
