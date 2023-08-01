using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class CadCheque
    {
        public int Id { get; set; }
        public string Caixa { get; set; }
        public string NumCheque { get; set; }
        public Nullable<decimal> Valor { get; set; }
        public string Obs { get; set; }
        public System.DateTime Data { get; set; }
    }
}
