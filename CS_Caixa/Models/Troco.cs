using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Troco
    {
        public int IdTroco { get; set; }
        public Nullable<int> IdAto { get; set; }
        public Nullable<int> IdReciboBalcao { get; set; }
        public System.DateTime Data { get; set; }
        public decimal Valor { get; set; }
    }
}
