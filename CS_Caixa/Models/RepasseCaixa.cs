using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class RepasseCaixa
    {
        public int RepasseId { get; set; }
        public System.DateTime DataCaixa { get; set; }
        public decimal ValorCaixa { get; set; }
        public System.DateTime DataPagamentoRepasse { get; set; }
        public decimal ValorRepasse { get; set; }
        public decimal ValorRestante { get; set; }
        public string Descricao { get; set; }
    }
}
