using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumirWSFactElect.ConsumirWSFE.ClasesResponse
{
    public class FEResponse
    {
        public ErroresResponse[] errores { get; set; }
        public EventosResponse[] eventos { get; set; }
        public CabResponse cabResp { get; set; }
        public DetResponse detResp { get; set; }
    }
}
