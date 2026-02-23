using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronicaProd.ConsumirWSFE.ClasesResponse
{
    public class FECotizacionResponse
    {
        public ErroresResponse[] errores { get; set; }
        public EventosResponse[] eventos { get; set; }

        public CotizacionRequest cotizacion { get; set; }
    }
}
