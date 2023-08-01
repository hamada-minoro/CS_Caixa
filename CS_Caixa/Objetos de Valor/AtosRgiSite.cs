using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Objetos_de_Valor
{
    public class AtosRgiSite : Ato
    {
        public string Qualificacao { get; set; }

        public int Protocolo { get; set; }

        public DateTime DataProtocolo { get; set; }

        public DateTime DataRegistro { get; set; }

        public int Recibo { get; set; }

        public string Matricula { get; set; }

        public string TipoLancamento { get; set; }

        public int NúmeroLancamento { get; set; }

        public decimal Distribuicao { get; set; }

        public decimal Prenotacao { get; set; }

        public decimal Buscas { get; set; }

    }
}
