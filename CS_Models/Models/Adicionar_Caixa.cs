using System;
using System.Collections.Generic;

namespace CS_Models.Models
{
    public partial class Adicionar_Caixa
    {
        public int Cod { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public string Atribuicao { get; set; }
        public string Descricao { get; set; }
        public Nullable<decimal> Valor { get; set; }
        public string TpPagamento { get; set; }
    }
}
