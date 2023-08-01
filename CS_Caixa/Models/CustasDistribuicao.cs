using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class CustasDistribuicao
    {
        public int Id_custas { get; set; }
        public Nullable<decimal> Emolumentos { get; set; }
        public Nullable<decimal> Fetj { get; set; }
        public Nullable<decimal> Fundperj { get; set; }
        public Nullable<decimal> Funperj { get; set; }
        public Nullable<decimal> Funarpen { get; set; }
        public Nullable<decimal> Pmcmv { get; set; }
        public Nullable<decimal> Iss { get; set; }
        public Nullable<int> Quant_Exced { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> VrFixo { get; set; }
        public Nullable<decimal> VrExced { get; set; }
        public Nullable<int> Ano { get; set; }
    }
}
