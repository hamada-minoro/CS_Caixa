using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Agragador
{
    public class Boleto
    {
        public int Boleto_id { get; set; }
        public string Protocolo { get; set; }
        public byte [] Arquivo { get; set; }
        public DateTime data_envio { get; set; }
        public string Visualizado { get; set; }
        public int QtdVisualizacao { get; set; }

    }
}
