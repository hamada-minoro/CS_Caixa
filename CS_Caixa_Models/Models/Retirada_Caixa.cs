using System;
using System.Collections.Generic;

namespace CS_Caixa_Models.Models
{
    public partial class Retirada_Caixa
    {
        public int Cod { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public string Descricao { get; set; }
        public Nullable<decimal> Valor { get; set; }
    }
}
