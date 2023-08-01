using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class ImportarMa
    {
        public int IdImportarMas { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public string Atribuicao { get; set; }
        public string TipoAto { get; set; }
        public string Selo { get; set; }
        public string Aleatorio { get; set; }
        public string TipoCobranca { get; set; }
        public Nullable<decimal> Emolumentos { get; set; }
        public Nullable<decimal> Fetj { get; set; }
        public Nullable<decimal> Fundperj { get; set; }
        public Nullable<decimal> Funperj { get; set; }
        public Nullable<decimal> Funarpen { get; set; }
        public Nullable<decimal> Ressag { get; set; }
        public Nullable<decimal> Mutua { get; set; }
        public Nullable<decimal> Acoterj { get; set; }
        public Nullable<decimal> Distribuidor { get; set; }
        public Nullable<decimal> Iss { get; set; }
        public Nullable<decimal> Total { get; set; }
    }
}
