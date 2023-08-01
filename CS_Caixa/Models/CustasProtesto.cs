using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class CustasProtesto
    {
        public int Id_Custas { get; set; }
        public int ORDEM { get; set; }
        public Nullable<int> ANO { get; set; }
        public string VAI { get; set; }
        public string TAB { get; set; }
        public string ITEM { get; set; }
        public string SUB { get; set; }
        public string DESCR { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        public string TEXTO { get; set; }
        public string TIPO { get; set; }
    }
}
