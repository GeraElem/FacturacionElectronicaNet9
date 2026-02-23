using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronicaProd.ConsumirWSFE.ClasesResponse
{
    public class FECompConsultarResult
    {
        public ErroresResponse[] errores { get; set; }
        public EventosResponse[] eventos { get; set; }
        public FECompConsultarResponse comprobante { get; set; }
    }
}
