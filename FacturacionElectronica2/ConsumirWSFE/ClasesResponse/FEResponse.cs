using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronica.ConsumirWSFE.ClasesResponse
{
    public class FEResponse
    {
        public ErroresResponse[] errores { get; set; }
        public EventosResponse[] eventos { get; set; }
        public CabResponse cabResp { get; set; }
        public List<DetResponse> detResp { get; set; }
    }
}
