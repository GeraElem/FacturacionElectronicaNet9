using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacturaElectronicaProd.ConsumirWSFE.ClasesRequest;

namespace FacturaElectronicaProd.ConsumirWSFE.ClasesResponse
{
    public class DocTipoResponse
    {
        public ErroresResponse[] errores { get; set; }
        public EventosResponse[] eventos { get; set; }

        public DocumentoRequest[] documentos { get; set; }
    }
}
