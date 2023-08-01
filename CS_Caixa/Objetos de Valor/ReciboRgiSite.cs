using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Objetos_de_Valor
{
    public class ReciboRgiSite : Ato
    {
        public int Recibo { get; set; }

        public DateTime DataPrazo { get; set; }

        public string Apresentante { get; set; }

        public string CpfCnpjApresentante { get; set; }

        public string Outorgado { get; set; }

        public string CpfCnpjOutorgado { get; set; }

        public int QtdPrenotacao { get; set; }

        public decimal ValorPrenotacao { get; set; }

        public int QtdBuscas { get; set; }

        public decimal ValorBuscas { get; set; }

        public string DescricaoValorOutros { get; set; }

        public decimal ValorOutros { get; set; }

        public string Observacao { get; set; }

        public int QtdAtos { get; set; }
    }
}
