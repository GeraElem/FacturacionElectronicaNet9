using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturaElectronicaProd.ConsumirWSFE.ClasesResponse
{
    public class FECompConsultarResponse
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
        public string fchVto { get; set; } //puede ser nulo
        public string monId { get; set; }
        public double monCotiz { get; set; }
        public string resultado { get; set; }
        public string cae { get; set; }
        public string fechaProceso { get; set; }
        public int ptoVta { get; set; }
        public int cbteTipo { get; set; }
        public ObsResponse[] observaciones { get; set; }
    }
}
