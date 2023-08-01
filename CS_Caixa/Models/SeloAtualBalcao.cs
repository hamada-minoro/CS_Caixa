using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class SeloAtualBalcao
    {
        public int IdSeloBalcao { get; set; }
        public string Letra { get; set; }
        public Nullable<int> Numero { get; set; }
    }
}
