using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class ItensCustasNota
    {
        public int Id_Custa { get; set; }
        public int Id_Ato { get; set; }
        public Nullable<int> Id_AtoNotas { get; set; }
        public string Tabela { get; set; }
        public string Item { get; set; }
        public string SubItem { get; set; }
        public string Quantidade { get; set; }
        public string Complemento { get; set; }
        public string Excessao { get; set; }
        public Nullable<decimal> Valor { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string Descricao { get; set; }
        public virtual Ato Ato { get; set; }
    }
}
