using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class ItensAtoNota
    {
        public int Id_AtoNotas { get; set; }
        public int Id_Ato { get; set; }
        public Nullable<int> Cont { get; set; }
        public Nullable<int> Protocolo { get; set; }
        public Nullable<int> Recibo { get; set; }
        public string TipoAto { get; set; }
        public string Natureza { get; set; }
        public Nullable<decimal> Emolumentos { get; set; }
        public Nullable<decimal> Fetj { get; set; }
        public Nullable<decimal> Fundperj { get; set; }
        public Nullable<decimal> Funperj { get; set; }
        public Nullable<decimal> Funarpen { get; set; }
        public Nullable<decimal> Pmcmv { get; set; }
        public Nullable<decimal> Iss { get; set; }
        public Nullable<decimal> Mutua { get; set; }
        public Nullable<decimal> Acoterj { get; set; }
        public Nullable<decimal> Distribuicao { get; set; }
        public Nullable<int> QuantDistrib { get; set; }
        public Nullable<decimal> Total { get; set; }
        public virtual Ato Ato { get; set; }
    }
}
