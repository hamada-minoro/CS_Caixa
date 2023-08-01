using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Objetos_de_Valor
{
    public class ConsultaApontado
    {
        public int ConsultaApontadoId { get; set; }
        public string IdAto { get; set; }
        public string Devedor { get; set; }
        public string DocumentoDevedor { get; set; }
        public DateTime DataProtocolo { get; set; }

        public string Protocolo { get; set; }
    }
}
