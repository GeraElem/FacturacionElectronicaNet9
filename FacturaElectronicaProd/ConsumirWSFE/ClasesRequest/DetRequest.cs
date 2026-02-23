using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronicaProd.ConsumirWSFE.ClasesRequest
{
    public class DetRequest
    {
        public int concepto { get; set; }
        public int docTipo { get; set; }
        public long docNro { get; set; }
        public long cbteDesde { get; set; }
        public long cbteHasta { get; set; }
        public string cbteFch { get; set; } //puede ser nulo
        public double impTotal { get; set; }
        public double impTotConc { get; set; }
        public double impNeto { get; set; }
        public double impOpEx { get; set; }
        public double impIVA { get; set; }
        public double impTrib { get; set; }
        public string fchServDesde { get; set; } //puede ser nulo
        public string fchServHasta { get; set; } //puede ser nulo
        public string fchVtoPago { get; set; } //puede ser nulo
        public string monId { get; set; }
        public double monCotiz { get; set; }
        public bool monCotizSpecified { get; set; }
        public string canMisMonExt { get; set; }
        public int condicionIVAReceptorId { get; set; }
        public CbtesAsocRequest[] cbtesAsoc { get; set; } //puede ser nulo
        public TributoRequest[] tributos { get; set; } //puede ser nulo
        public IvaRequest[] iva { get; set; } //puede ser nulo

        public OpcionalRequest[] opcional { get; set; } //puede ser nulo
    }
}
