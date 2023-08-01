using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Enotariado
    {
        public int IdEnotariado { get; set; }
        public Nullable<int> IdAto { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public Nullable<decimal> Valor { get; set; }
    }
}
